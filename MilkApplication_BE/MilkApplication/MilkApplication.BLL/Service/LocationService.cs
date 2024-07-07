using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models.PaginationDTO;

namespace MilkApplication.BLL.Service
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> AddLocationAsync(LocationDTO locationDTO)
        {
            var locationObj = _mapper.Map<Location>(locationDTO);
            await _unitOfWork.LocationRepository.AddAsync(locationObj);
            await _unitOfWork.SaveChangeAsync();

            var response = new ResponseDTO
            {
                IsSucceed = true,
                Message = "Location added successfully",
            };
            return response;
        }

        public async Task<ResponseDTO> DeleteLocationAsync(int id)
        {
            var deleteLocation = await _unitOfWork.LocationRepository.GetByIdAsync(id);
            if (deleteLocation != null)
            {
                await _unitOfWork.LocationRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Location deleted successfully"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = $"Location with ID {id} not found"
                };
            }
        }

        public async Task<List<LocationDTO>> GetAllLocationsAsync()
        {
            var locationGetAll = await _unitOfWork.LocationRepository.GetAllAsync();
            var locationMapper = _mapper.Map<List<LocationDTO>>(locationGetAll);
            return locationMapper;
        }

        public async Task<LocationDTO> GetLocationByIdAsync(int id)
        {
            var locationFound = await _unitOfWork.LocationRepository.GetByIdAsync(id);
            if (locationFound == null)
            {
                return null;
            }
            var locationMapper = _mapper.Map<LocationDTO>(locationFound);
            return locationMapper;
        }

        public async Task<ResponseDTO> UpdateLocationAsync(int id, LocationDTO locationDTO)
        {
            var locationUpdate = await _unitOfWork.LocationRepository.GetByIdAsync(id);
            if (locationUpdate != null)
            {
                locationUpdate = _mapper.Map(locationDTO, locationUpdate);
                await _unitOfWork.LocationRepository.UpdateAsync(locationUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = true,
                        Message = "Location update successfully!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Location update failed!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Location not found!"
            };
        }
        public async Task<Pagination<LocationDTO>> GetLocationByFilterAsync(PaginationParameter paginationParameter, LocationFilterDTO locationFilterDTO)
        {
            try
            {
                var locations = await _unitOfWork.LocationRepository.GetLocationByFilterAsync(paginationParameter, locationFilterDTO);
                if (locations != null)
                {
                    var mapperResult = _mapper.Map<List<LocationDTO>>(locations);
                    return new Pagination<LocationDTO>(mapperResult, locations.TotalCount, locations.CurrentPage, locations.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
