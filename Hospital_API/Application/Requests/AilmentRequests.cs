using Hospital_API.DTOs;
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

    public class GetSingleAilmentRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllAilmentRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
