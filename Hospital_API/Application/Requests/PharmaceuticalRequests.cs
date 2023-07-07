using Hospital_API.DTOs;
using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class AddPharmaceuticalRequest : IRequest<ResponseModelView>
    {
        public PharmaceuticalDto? PharmaceuticalDto { get; set; }
    }

    public class UpdatePharmaceuticalRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
        public PharmaceuticalDto? PharmaceuticalDto { get; set; }
    }

    public class CheckPharmaceuticalExistRequest : IRequest<ResponseModelView>
    {
        public int PharmaceuticalId { get; set; }
    }

    public class CheckPharmaceuticalNameExistRequest : IRequest<ResponseModelView>
    {
        public int CategoryId { get; set; }
        public int PharmaceuticalId { get; set; }
        public string? Name { get; set; }
    }

    public class GetSinglePharmaceuticalRequest : IRequest<ResponseModelView>
    {
        public int Id { get; set; }
    }

    public class GetAllPharmaceuticalRequest : IRequest<ResponseModelView>
    {
        public bool? Active { get; set; }
    }
}
