using AutoMapper;
using StackOverflow.Domain.Entities;
using StackOverflow.Web.Controllers;
using StackOverflow.Web.Models;

namespace StackOverflow.Web
{
    public static class AutoMapperConfig
    {
        public static void RegisterMaps()
        {
            Mapper.CreateMap<AccountRegisterModel, Account>().ReverseMap();
            Mapper.CreateMap<AccountLoginModel, Account>().ReverseMap();
            Mapper.CreateMap<NewQuestionModel, Question>().ReverseMap();
            Mapper.CreateMap<QuestionDetailModel, Question>().ReverseMap();
        }
    }
}