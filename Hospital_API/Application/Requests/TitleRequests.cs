using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddTitleRequest : IRequest<ResponseModelView>
    {
        public TitleDto? TitleDto { get; set; }
    }

    public class UpdateTitleRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public TitleDto? TitleDto { get; set; }
    }

    public class GetSingleTitleRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllTitleRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
