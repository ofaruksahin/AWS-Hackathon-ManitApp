using ManitApp.API.Application.RequestModels;
using ManitApp.API.Application.ResponseModels;

namespace ManitApp.API.Application.Services.Contracts
{
    public interface IVectorizeService
    {
        Task VectorizeOrderForUser(int userId);
        Task<List<SuggestionResponseModel>> GetSuggestions(SuggestionRequestModel requestModel);
    }
}
