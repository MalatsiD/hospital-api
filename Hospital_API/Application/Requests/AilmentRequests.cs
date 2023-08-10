using Hospital_API.DTOs;
using Hospital_API.DTOs.Filters;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddAilmentRequest : IRequest<ResponseModelView>
    {
        public AilmentDto? AilmentDto { get; set; }
    }

    public class UpdateAilmentRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public AilmentDto? AilmentDto { get; set; }
    }

    public class UpdateAilmentStatusRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public StatusChangeDto? StatusChangeDto { get; set; }
    }

    public class DeleteAilmentRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class CheckAilmentNameExistRequest : IRequest<ResponseModelView>
    {
        public int AilmentId { get; set; }
        public string? Name { get; set; }
    }

    public class CheckAilmentExistRequest : IRequest<ResponseModelView>
    {
        public int AilmentId { get; set; }
    }

    public class GetSingleAilmentRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllAilmentListRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; } = true;
    }

    public class GetAllAilmentRequest : IRequest<ResponsePaginationModelView>
    {
        public AilmentFilterDto? ailmentFilterDto { get; set; }
    }
}
