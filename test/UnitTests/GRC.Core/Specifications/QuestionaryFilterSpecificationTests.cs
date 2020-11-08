using GRC.Core.Entities;
using GRC.Core.Specifications;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace UnitTests.GRCCore.Specifications
{
    public class QuestionaryFilterSpecificationTests
    {

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void GetById_Should_Return_Object_On_Correct_Data(int questionaryId)
        {
            var spec = new QuestionaryFilterSpecification(questionaryId);

            var result = GetTestQuestionaryCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Single(result);
            Assert.Equal(questionaryId, result.FirstOrDefault().Id);
        }

        [Theory]
        [InlineData("Owner",1)]
        [InlineData("Owner",2)]
        [InlineData("Owner1",3)]
        public void GetByIdOwner_Should_Return_Object_On_Correct_Data(string uid, int questionaryId)
        {
            var spec = new QuestionaryFilterSpecification(uid,questionaryId);

            var result = GetTestQuestionaryCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());


            Assert.Single(result);
            Assert.Equal(questionaryId, result.FirstOrDefault().Id);
            Assert.Equal(uid, result.FirstOrDefault().OwnerUid);
        }
        
                    [Theory]
        [InlineData("Owner",false, 2)]
        [InlineData("Owner1",false, 2)]
        [InlineData("Admin",true, 4)]
        public void GetByUid_Should_Return_Object_On_Correct_Data(string uid, bool isAdministrator,int responseCount)
        {
            var spec = new QuestionaryFilterSpecification(uid, isAdministrator);

            var result = GetTestQuestionaryCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());
                             

            Assert.Equal(responseCount,result.Count());
        }

        [Theory]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void GetById_Should_Return_No_Object_On_Incorrect_Data(int questionaryId)
        {
            var spec = new QuestionaryFilterSpecification(questionaryId);

            var result = GetTestQuestionaryCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Equal(0,result.Count());
        }

        [Theory]
        [InlineData("Owner2", 1)]
        [InlineData("Owner2", 2)]
        [InlineData("Owner2", 3)]
        public void GetByIdOwner_Should_Return_No_Object_On_Incorrect_Data(string uid, int questionaryId)
        {
            var spec = new QuestionaryFilterSpecification(uid, questionaryId);

            var result = GetTestQuestionaryCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());

            Assert.Equal(0, result.Count());
        }

        [Theory]
        [InlineData("Owner2", false)]
        [InlineData("Owner3", false)]
        public void GetByUid_Should_Return_No_Object_On_Incorrect_Data(string uid, bool isAdministrator)
        {
            var spec = new QuestionaryFilterSpecification(uid, isAdministrator);

            var result = GetTestQuestionaryCollection()
                             .AsQueryable()
                             .Where(spec.WhereExpressions.FirstOrDefault());


            Assert.Equal(0, result.Count());
        }




        public List<Questionary> GetTestQuestionaryCollection()
        {
            return new List<Questionary>()
            {
               new Questionary("Owner"){Id=1, StandardId=1 },
               new Questionary("Owner"){Id=2, StandardId=1 },
               new Questionary("Owner1"){Id=3, StandardId=1 },
               new Questionary("Owner1"){Id=4, StandardId=1 },
            };
        }
    }
}
