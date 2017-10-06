using System.Collections.Generic;
using QuestionServiceWebApi;

namespace PairingTest.Unit.Tests.QuestionServiceWebApi.Builders
{
    public class QuestionnaireBuilder
    {
        private readonly Questionnaire _questionnaire;

        public QuestionnaireBuilder()
        {
            _questionnaire = new Questionnaire()
            {
                QuestionsText = new List<string>()
            };
        }

        public QuestionnaireBuilder WithTitle(string title)
        {
            _questionnaire.QuestionnaireTitle = title;

            return this;
        }

        public QuestionnaireBuilder WithQuestion(string question)
        {
            _questionnaire.QuestionsText.Add(question);

            return this;
        }

        public Questionnaire Build()
        {
            return _questionnaire;
        }
    }
}
