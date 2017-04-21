﻿using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MoneyFox.DataAccess.Entities;
using MoneyFox.DataAccess.Infrastructure;
using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.Constants;
using Xunit;

namespace MoneyFox.DataAccess.Tests.Repositories
{
    public class CategoryRepositoryTests : IDisposable
    {
        /// <summary>
        ///     Setup Logic who is executed before every test
        /// </summary>
        public CategoryRepositoryTests()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.Migrate();
            }
        }

        /// <summary>
        ///     Cleanup logic who is executed after executign every test.
        /// </summary>
        public void Dispose()
        {
            var path = Path.Combine(AppContext.BaseDirectory, DatabaseConstants.DB_NAME);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        [Fact]
        public async void Add_AddedAndRead()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(testEntry.Name, loadedEntry.Name);
        }

        [Fact]
        public async void Add_AddMultipleEntries()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);

            // Act
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(3, repository.GetAll().Count());
        }

        [Fact]
        public async void Add_AddNewEntryOnEveryCall()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();
            testEntry.Id = 0;
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(2, repository.GetAll().Count());
        }

        [Fact]
        public async void Add_IdSet()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.NotNull(testEntry.Id);
            Assert.NotEqual(0, testEntry.Id);
        }

        [Fact]
        public async void Update_EntryUpdated()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);

            var newValue = "newText";
            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            testEntry.Name = newValue;
            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            var loadedEntry = await repository.GetById(testEntry.Id);
            Assert.Equal(newValue, loadedEntry.Name);
        }

        [Fact]
        public async void Update_IdUnchanged()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            var idBeforeUpdate = testEntry.Id;
            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(idBeforeUpdate, testEntry.Id);
        }

        [Fact]
        public async void Update_NoNewEntryAdded()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);

            var testEntry = new CategoryEntity
            {
                Name = "Testtext"
            };

            // Act
            repository.Add(testEntry);
            await unitOfWork.Commit();

            repository.Update(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public async void Delete_EntryDeleted()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);
            var testEntry = new CategoryEntity();
            repository.Add(testEntry);
            await unitOfWork.Commit();

            // Act
            repository.Delete(testEntry);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(0, repository.GetAll().Count());
        }

        [Fact]
        public async void Delete_AssignedPaymentsSetNull()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var categoryRepository = new CategoryRepository(factory);
            var paymentRepository = new PaymentRepository(factory);

            var category = new CategoryEntity();
            var payment = new PaymentEntity
            {
                ChargedAccount = new AccountEntity(),
                Category = category
            };
            categoryRepository.Add(category);
            paymentRepository.Add(payment);
            await unitOfWork.Commit();

            // Act
            categoryRepository.Delete(category);
            await unitOfWork.Commit();

            // Assert
            Assert.Null(payment.Category);
            Assert.Null(paymentRepository.GetById(payment.Id).Result.Category);
        }

        [Fact]
        public async void Delete_EntryNotFound()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);
            var testEntry = new CategoryEntity();

            // Act
            repository.Delete(testEntry);

            // Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await unitOfWork.Commit());
        }


        [Fact]
        public async void Delete_EntryMatchedFilterDeleted()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var filterText = "Text";
            var repository = new CategoryRepository(factory);
            var testEntry1 = new CategoryEntity { Name = filterText };
            var testEntry2 = new CategoryEntity();
            repository.Add(testEntry1);
            repository.Add(testEntry2);
            await unitOfWork.Commit();

            // Act
            repository.Delete(x => x.Name == filterText);
            await unitOfWork.Commit();

            // Assert
            Assert.Equal(1, repository.GetAll().Count());
        }

        [Fact]
        public void GetAll_NoData()
        {
            // Arrange
            var repository = new CategoryRepository(new DbFactory());

            // Act
            var emptyList = repository.GetAll().ToList();

            // Assert
            Assert.NotNull(emptyList);
            Assert.False(emptyList.Any());
        }

        [Fact]
        public async void GetAll_AllDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetAll().ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(3, resultList.Count);
        }

        [Fact]
        public async void GetMany_NothingMatched()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetMany(x => x.Name == "text").ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.False(resultList.Any());
        }

        [Fact]
        public async void GetMany_MatchedDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);
            var filterText = "Text";
            repository.Add(new CategoryEntity { Name = filterText });
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            await unitOfWork.Commit();

            // Act
            var resultList = repository.GetMany(x => x.Name == filterText).ToList();

            // Assert
            Assert.NotNull(resultList);
            Assert.Equal(1, resultList.Count);
        }

        [Fact]
        public async void Get_NothingMatched()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            await unitOfWork.Commit();

            // Act
            var result = await repository.Get(x => x.Name == "text");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async void Get_MatchedDataReturned()
        {
            // Arrange
            var factory = new DbFactory();
            var unitOfWork = new UnitOfWork(factory);

            var repository = new CategoryRepository(factory);
            var filterText = "Text";
            var testEntry = new CategoryEntity { Name = filterText };
            repository.Add(testEntry);
            repository.Add(new CategoryEntity());
            repository.Add(new CategoryEntity());
            await unitOfWork.Commit();

            // Act
            var result = await repository.Get(x => x.Name == filterText);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testEntry.Id, result.Id);
        }
    }
}