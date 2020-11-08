using GRC.Core.Entities;
using GRC.Core.Specifications;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.GRCCore.Specifications
{
    public class QuestionFilterSpecificationTests
    {

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        [InlineData(3, 1)]
        public void GetByControlId_Should_Return_List_On_Correct_Data(int controlId, int returnCount)
        {

            var spec = new QuestionFilterSpecification(controlId);

            var result = GetTestQuestionCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());


            Assert.Equal(returnCount, result.Count());
            Assert.Equal(controlId, result.FirstOrDefault().ControlId);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void GetById_Should_Return_Object_On_Correct_Data(int controlId, int questionId)
        {
            var spec = new QuestionFilterSpecification(controlId, questionId);

            var result = GetTestQuestionCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Single(result);
            Assert.Equal(controlId, result.FirstOrDefault().ControlId);
            Assert.Equal(questionId, result.FirstOrDefault().Id);
            Assert.Single(result.FirstOrDefault().QuestionAnswers);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void GetByControlId_Should_Return_No_Object_On_Incorrect_Data(int questionaryId)
        {
            var spec = new QuestionFilterSpecification(questionaryId);

            var result = GetTestQuestionCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Equal(0, result.Count());
        }

        [Theory]
        [InlineData(1, 3)]
        [InlineData(1, 0)]
        [InlineData(0, 3)]
        public void GetById_Should_Return_No_Object_On_Incorrect_Data(int controlId, int questionId)
        {
            var spec = new QuestionFilterSpecification(controlId, questionId);

            var result = GetTestQuestionCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Equal(0, result.Count());
        }

        public List<Question> GetTestQuestionCollection() => new List<Question>()
        {
            new Question(){Id=1, ControlId=1, QuestionAnswers= new List<QuestionAnswer>{ new QuestionAnswer { Id = 1, AnswerText = "Text" } } },
            new Question(){Id=2, ControlId=1 , QuestionAnswers= new List<QuestionAnswer>{ new QuestionAnswer { Id = 2, AnswerText = "Text" } } },
            new Question(){Id=3, ControlId=2 , QuestionAnswers= new List<QuestionAnswer>{ new QuestionAnswer { Id = 3, AnswerText = "Text" } } },
            new Question(){Id=4, ControlId=2 , QuestionAnswers= new List<QuestionAnswer>{ new QuestionAnswer { Id = 4, AnswerText = "Text" } } },
            new Question(){Id=5, ControlId=3 , QuestionAnswers= new List<QuestionAnswer>{ new QuestionAnswer { Id = 5, AnswerText = "Text" } } }
        };
    }
}
