using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddGenderRequest : IRequest<ResponseModelView>
    {
        public GenderDto? GenderDto { get; set; }
    }

    public class UpdateGenderRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public GenderDto? GenderDto { get; set; }
    }

    public class UpdateGenderStatusRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public StatusChangeDto? StatusChangeDto { get; set; }
    }

    public class DeleteGenderRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class CheckGenderExistRequest : IRequest<ResponseModelView>
    {
        public int GenderId { get; set; }
    }

    public class CheckGenderNameExistRequest : IRequest<ResponseModelView>
    {
        public int GenderId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSingleGenderRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllGenderListRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; } = true;
    }

    public class GetAllGenderRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
