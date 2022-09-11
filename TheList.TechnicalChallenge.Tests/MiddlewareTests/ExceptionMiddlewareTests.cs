using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using TheList.TechnicalChallenge.Exceptions;
using TheList.TechnicalChallenge.Middleware;
using Xunit;

namespace TheList.TechnicalChallenge.Tests.MiddlewareTests
{
    public class ExceptionMiddlewareTests
    {
        Mock<ILogger<ExceptionMiddleware>> _mockLogger = new Mock<ILogger<ExceptionMiddleware>>();

        [Fact]
        public async Task InvokeAsync_IfCustomValidatoinExceptionThrown_ReturnsBadRequestStatusCode()
        {
            //Arrange
            var expectedException = new CustomValidationException();            
            var httpContext = new DefaultHttpContext();
            var expectedStatusCode = HttpStatusCode.BadRequest;
            Task MockNextMiddleware(HttpContext _)
            {
                return Task.FromException(expectedException);
            }
            //Act
            var sut = new ExceptionMiddleware(MockNextMiddleware);
            await sut.InvokeAsync(httpContext, _mockLogger.Object);
            //Assert
            Assert.Equal(expectedStatusCode, (HttpStatusCode)httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_IfCheckoutNotFoundExceptionThrown_ReturnsBadRequestStatusCode()
        {
            //Arrange
            var expectedException = new CheckoutNotFoundException(123);
            var httpContext = new DefaultHttpContext();
            var expectedStatusCode = HttpStatusCode.BadRequest;
            Task MockNextMiddleware(HttpContext _)
            {
                return Task.FromException(expectedException);
            }
            //Act
            var sut = new ExceptionMiddleware(MockNextMiddleware);
            await sut.InvokeAsync(httpContext, _mockLogger.Object);
            //Assert
            Assert.Equal(expectedStatusCode, (HttpStatusCode)httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_IfUndefinedExceptionThrown_ReturnsInternalServerErrorCode()
        {
            //Arrange
            var expectedException = new ArgumentNullException("Argument can not be null");
            var httpContext = new DefaultHttpContext();
            var expectedStatusCode = HttpStatusCode.InternalServerError;
            Task MockNextMiddleware(HttpContext _)
            {
                return Task.FromException(expectedException);
            }
            //Act
            var sut = new ExceptionMiddleware(MockNextMiddleware);
            await sut.InvokeAsync(httpContext, _mockLogger.Object);
            //Assert
            Assert.Equal(expectedStatusCode, (HttpStatusCode)httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_If_PositiveExecutionPath_Returns_SuccessCode()
        {
            //Arrange
            var expectedException = new ArgumentNullException("Argument can not be null");
            var httpContext = new DefaultHttpContext();
            var expectedStatusCode = HttpStatusCode.OK;
            Task MockNextMiddleware(HttpContext _)
            {
                return Task.FromResult(0);
            }
            //Act
            var sut = new ExceptionMiddleware(MockNextMiddleware);
            await sut.InvokeAsync(httpContext, _mockLogger.Object);
            //Assert
            Assert.Equal(expectedStatusCode, (HttpStatusCode)httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_IfCustomValidatoinExceptionThrownWithFailuresDict_ReturnsBadRequestStatusCode()
        {
            //Arrange
            var stubDictionary = new Dictionary<string, string[]>();
            var expectedException = new CustomValidationException(stubDictionary);
            var httpContext = new DefaultHttpContext();
            var expectedStatusCode = HttpStatusCode.BadRequest;
            Task MockNextMiddleware(HttpContext _)
            {
                return Task.FromException(expectedException);
            }
            //Act
            var sut = new ExceptionMiddleware(MockNextMiddleware);
            await sut.InvokeAsync(httpContext, _mockLogger.Object);
            //Assert
            Assert.Equal(expectedStatusCode, (HttpStatusCode)httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_IfCustomValidatoinExceptionThrownWithValidatoinFailures_ReturnsBadRequestStatusCode()
        {
            //Arrange
            var stubValidationFailures = new List<ValidationFailure>();
            var expectedException = new CustomValidationException(stubValidationFailures);
            var httpContext = new DefaultHttpContext();
            var expectedStatusCode = HttpStatusCode.BadRequest;
            Task MockNextMiddleware(HttpContext _)
            {
                return Task.FromException(expectedException);
            }
            //Act
            var sut = new ExceptionMiddleware(MockNextMiddleware);
            await sut.InvokeAsync(httpContext, _mockLogger.Object);
            //Assert
            Assert.Equal(expectedStatusCode, (HttpStatusCode)httpContext.Response.StatusCode);
        }

        [Fact]
        public async Task InvokeAsync_IfCustomValidatoinExceptionThrownWithValidatoinMessage_ReturnsBadRequestStatusCode()
        {
            //Arrange
           
            var expectedException = new CustomValidationException("Some validation exception occured!");
            var httpContext = new DefaultHttpContext();
            var expectedStatusCode = HttpStatusCode.BadRequest;
            Task MockNextMiddleware(HttpContext _)
            {
                return Task.FromException(expectedException);
            }
            //Act
            var sut = new ExceptionMiddleware(MockNextMiddleware);
            await sut.InvokeAsync(httpContext, _mockLogger.Object);
            //Assert
            Assert.Equal(expectedStatusCode, (HttpStatusCode)httpContext.Response.StatusCode);
        }

    }
}
