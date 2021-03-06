﻿using FluentAssertions;
using Kros.Data.BulkActions;
using System;
using System.Data.SqlClient;
using Xunit;

namespace Kros.Utils.UnitTests.Data
{
    public class BulkActionFactoriesShould
    {
        [Fact]
        public void GetFactoryByConnection()
        {
            using (var conn = new SqlConnection())
            {
                var factory = BulkActionFactories.GetFactory(conn);

                factory.Should().NotBeNull();
            }
        }

        [Fact]
        public void GetFactoryByAdoClientName()
        {
            var factory = BulkActionFactories.GetFactory("connectionstring", "System.Data.SqlClient");

            factory.Should().NotBeNull();
        }

        [Fact]
        public void ThrowExceptionWhenConnectionIsNotRegistered()
        {
            using (var conn = new CustomConnection())
            {
                Action action = () => { var factory = BulkActionFactories.GetFactory(conn); };

                action.ShouldThrow<InvalidOperationException>()
                    .WithMessage($"{nameof(IBulkActionFactory)} for connection type 'CustomConnection' is not registered.");
            }
        }

        [Fact]
        public void ThrowExceptionWhenAdoClientNameIsNotRegistered()
        {
            Action action = () => { var factory = BulkActionFactories.GetFactory("constring", "System.Data.CustomClient"); };

            action.ShouldThrow<InvalidOperationException>()
                .WithMessage($"{nameof(IBulkActionFactory)} for ADO client 'System.Data.CustomClient' is not registered.");
        }
    }
}
