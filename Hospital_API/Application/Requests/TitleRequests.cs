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

    public class UpdateTitleStatusRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public StatusChangeDto? StatusChangeDto { get; set; }
    }

    public class DeleteTitleRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class CheckTitleExistRequest : IRequest<ResponseModelView>
    {
        public int TitleId { get; set; }
    }

    public class CheckTitleNameExistRequest : IRequest<ResponseModelView>
    {
        public int TitleId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSingleTitleRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllTitleListRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; } = true;
    }

    public class GetAllTitleRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
