using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddressService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> AddAddressAsync(AddressDTO addressDTO)
        {
            var user = await _unitOfWork.UserRepository.GetById(addressDTO.Id);
            if (user == null)
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "User not found",
                };
            }
            var addressObj = _mapper.Map<Address>(addressDTO);

            addressObj.User = user;

            await _unitOfWork.AddressRepository.AddAsync(addressObj);
            await _unitOfWork.SaveChangeAsync();

            var response = new ResponseDTO
            {
                IsSucceed = true,
                Message = "Address added successfully",
            };
            return response;
        }
        public async Task<List<AddressDTO>> GetAllAddressAsync()
        {
            var addressGetAll = await _unitOfWork.AddressRepository.GetAllAsync();
            var addressMapper = _mapper.Map<List<AddressDTO>>(addressGetAll);
            return addressMapper;
        }
        public async Task<ResponseDTO> UpdateAddressAsync(int id, AddressDTO addressDTO)
        {
            var addressUpdate = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            if (addressUpdate != null)
            {
                addressUpdate = _mapper.Map(addressDTO, addressUpdate);
                await _unitOfWork.CommentRepository.UpdateAsync(addressUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = true,
                        Message = "Address update successfully!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Address update failed!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Address not found!"
            };
        }

        public async Task<ResponseDTO> DeleteAddressAsync(int id)
        {
            var deleteAddress = await _unitOfWork.AddressRepository.GetByIdAsync(id);
            if (deleteAddress != null)
            {
                await _unitOfWork.AddressRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Address deleted successfully"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = $"Address with ID {id} not found"
                };
            }

        }
    }
}