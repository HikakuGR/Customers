using System;
using Xunit;
using BlazorApp;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Services;
using MongoDB.Bson;
using Mongo2Go;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using BlazorApp.Controllers;
using System.Collections.Generic;

namespace BlazorApp.Tests
{
    

    public class BasicTests
    {



        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.Equal(5, Add(2, 2));
        }

        int Add(int x, int y)
        {
            return x + y;
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        public void MyFirstTheory(int value)
        {
            Assert.True(IsOdd(value));
        }

        bool IsOdd(int value)
        {
            return value % 2 == 1;
        }


    }
}
