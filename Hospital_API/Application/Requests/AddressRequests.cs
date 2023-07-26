﻿using Hospital_API.ViewModels;
using MediatR;

namespace Hospital_API.Application.Requests
{
    public class CheckCityInAddressExistRequest : IRequest<ResponseModelView>
    {
        public int CityId { get; set; }
    }
}