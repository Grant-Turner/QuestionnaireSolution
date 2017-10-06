using Moq;
using NUnit.Framework;
using PairingTest.Web.Controllers;
using PairingTest.Web.Models;
using PairingTest.Web.Wrappers.Interfaces;

namespace PairingTest.Unit.Tests.Web
{
    [TestFixture]
    public class QuestionnaireControllerTests
    {
        private QuestionnaireController _questionnaireController;
        private Mock<IHttpClientWrapper> _httpClientWrapperMock;

        [Test]
        public void Index_WhenCalled_ReturnsViewWithQuestionnaireViewModel()
        {
            //Arrange
            SetupQuestionnaireController();
            var expectedQuestionnaire = new QuestionnaireViewModel() { QuestionnaireTitle = "My expected quesitons" };
            _httpClientWrapperMock.Setup(m => m.GetAsync<QuestionnaireViewModel>("Questions/Get")).Returns(expectedQuestionnaire);

            //Act
            var result = (QuestionnaireViewModel)_questionnaireController.Index().Result.ViewData.Model;

            //Assert
            Assert.That(result.QuestionnaireTitle, Is.EqualTo(expectedQuestionnaire.QuestionnaireTitle));
        }

        private void SetupQuestionnaireController()
        {
            _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            _questionnaireController = new QuestionnaireController(_httpClientWrapperMock.Object);
        }
    }
}