using AutoMapper;
using FinalSurveyPractice.DTOs.AuthUser;
using FinalSurveyPractice.DTOs.Category;
using FinalSurveyPractice.DTOs.Question;
using FinalSurveyPractice.DTOs.QuestionAnswer;
using FinalSurveyPractice.DTOs.Role;
using FinalSurveyPractice.DTOs.Survey;
using FinalSurveyPractice.DTOs.UserAnswer;
using FinalSurveyPractice.Models;

namespace FinalSurveyPractice
{
    public class AutoMapperPorfile : Profile
    {
        public AutoMapperPorfile()
        {
            //User mapping
            CreateMap<User, GetUserDto>();

            //Category mapping
            CreateMap<Category, GetCategoryDto>();
            CreateMap<AddCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();

            //Question mapping
            CreateMap<Question, GetQuestionDto>();
            CreateMap<AddQuestionDto, Question>();
            CreateMap<UpdateQuestionDto, Question>();

            //QuestionAnswer mapping
            CreateMap<QuestionAnswer, GetQuestionAnswerDto>();
            CreateMap<AddQuestionAnswerDto, QuestionAnswer>();
            CreateMap<UpdateQuestionAnswerDto, QuestionAnswer>();

            //Role mapping
            CreateMap<Role, GetRoleDto>();
            CreateMap<AddRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();

            //Survey mapping
            CreateMap<Survey, GetSurveyDto>();
            CreateMap<AddSurveyDto, Survey>();
            CreateMap<UpdateSurveyDto, Survey>();

            //UserAnswer mapping
            CreateMap<UserAnswer, GetUserAnswerDto>();
            CreateMap<AddUserAnswerDto, UserAnswer>();
            CreateMap<UpdateUserAnswerDto, UserAnswer>();
        }
    }
}
