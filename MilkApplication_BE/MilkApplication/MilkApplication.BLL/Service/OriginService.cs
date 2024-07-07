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
    public class OriginService : IOriginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OriginService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> AddOriginAsync(OriginDTO originDTO)
        {
            var originObj = _mapper.Map<Origin>(originDTO);
            await _unitOfWork.OriginRepository.AddAsync(originObj);
            await _unitOfWork.SaveChangeAsync();

            var response = new ResponseDTO
            {
                IsSucceed = true,
                Message = "Origin added successfully",
            };
            return response;
        }

        public async Task<ResponseDTO> DeleteOriginAsync(int id)
        {
            var deleteOrigin = await _unitOfWork.OriginRepository.GetByIdAsync(id);
            if (deleteOrigin != null)
            {
                await _unitOfWork.OriginRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Origin deleted successfully"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = $"Origin with ID {id} not found"
                };
            }
        }

        public async Task<List<OriginDTO>> GetAllOriginsAsync()
        {
            var originGetAll = await _unitOfWork.OriginRepository.GetAllAsync();
            var originMapper = _mapper.Map<List<OriginDTO>>(originGetAll);
            return originMapper;
        }

        public async Task<OriginDTO> GetOriginByIdAsync(int id)
        {
            var originFound = await _unitOfWork.OriginRepository.GetByIdAsync(id);
            if (originFound == null)
            {
                return null;
            }
            var originMapper = _mapper.Map<OriginDTO>(originFound);
            return originMapper;
        }

        public async Task<ResponseDTO> UpdateOriginAsync(int id, OriginDTO originDTO)
        {
            var originUpdate = await _unitOfWork.OriginRepository.GetByIdAsync(id);
            if (originUpdate != null)
            {
                originUpdate = _mapper.Map(originDTO, originUpdate);
                await _unitOfWork.OriginRepository.UpdateAsync(originUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = true,
                        Message = "Origin update successfully!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Origin update failed!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Origin not found!"
            };
        }
        public async Task<Pagination<OriginDTO>> GetOriginByFilterAsync(PaginationParameter paginationParameter, OriginFilterDTO originFilterDTO)
        {
            try
            {
                var origins = await _unitOfWork.OriginRepository.GetOriginByFilterAsync(paginationParameter, originFilterDTO);
                if (origins != null)
                {
                    var mapperResult = _mapper.Map<List<OriginDTO>>(origins);
                    return new Pagination<OriginDTO>(mapperResult, origins.TotalCount, origins.CurrentPage, origins.PageSize);
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
