using Moq;
using NUnit.Framework;
using PairingTest.Unit.Tests.QuestionServiceWebApi.Builders;
using QuestionServiceWebApi;
using QuestionServiceWebApi.Controllers;
using QuestionServiceWebApi.Interfaces;

namespace PairingTest.Unit.Tests.QuestionServiceWebApi
{
    [TestFixture]
    public class QuestionsControllerTests
    {
        private QuestionsController _questionsController;
        private Mock<IQuestionRepository> _mockQuestionRepository;

        [Test]
        public void Get_WhenCalled_ReturnsQuestionnaire()
        {
            //Arrange
            SetupQuestionsController();
            var questionnaire = new QuestionnaireBuilder()
                    .WithTitle("Title")
                    .WithQuestion("Q1")
                    .WithQuestion("Q2")
                    .WithQuestion("Q3")
                    .Build();
            _mockQuestionRepository.Setup(m => m.GetQuestionnaire()).Returns(questionnaire);

            //Act
            var questions = _questionsController.Get();

            //Assert
            _mockQuestionRepository.Verify(m => m.GetQuestionnaire(), Times.Once);
            Assert.IsInstanceOf<Questionnaire>(questions);
            Assert.That(questions, Is.EqualTo(questionnaire));
        }

        private void SetupQuestionsController()
        {
            _mockQuestionRepository = new Mock<IQuestionRepository>();
            _questionsController = new QuestionsController(_mockQuestionRepository.Object);
        }
    }    
}