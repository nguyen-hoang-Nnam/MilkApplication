using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class VouchersService : IVouchersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VouchersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDTO> AddVouchersAsync(VouchersDTO vouchersDTO)
        {
            var vouchersObj = _mapper.Map<Vouchers>(vouchersDTO);
            await _unitOfWork.VouchersRepository.AddAsync(vouchersObj);
            await _unitOfWork.SaveChangeAsync();

            var response = new ResponseDTO
            {
                IsSucceed = true,
                Message = "Voucher added successfully",
            };
            return response;
        }

        public async Task<ResponseDTO> DeleteVouchersAsync(int id)
        {
            var deleteVouchers = await _unitOfWork.VouchersRepository.GetByIdAsync(id);
            if (deleteVouchers != null)
            {
                await _unitOfWork.VouchersRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Vouchers deleted successfully"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = $"Vouchers with ID {id} not found"
                };
            }
        }

        public async Task<List<VouchersDTO>> GetAllVouchersAsync()
        {
            var vouchersGetAll = await _unitOfWork.VouchersRepository.GetAllAsync();
            var vouchersMapper = _mapper.Map<List<VouchersDTO>>(vouchersGetAll);
            return vouchersMapper;
        }

        public async Task<VouchersDTO> GetVouchersByIdAsync(int id)
        {
            var vouchersFound = await _unitOfWork.VouchersRepository.GetByIdAsync(id);
            if (vouchersFound == null)
            {
                return null;
            }
            var vouchersMapper = _mapper.Map<VouchersDTO>(vouchersFound);
            return vouchersMapper;
        }

        public async Task<ResponseDTO> UpdateVouchersAsync(int id, VouchersDTO voucherDTO)
        {
            var voucherUpdate = await _unitOfWork.VouchersRepository.GetByIdAsync(id);
            if (voucherUpdate != null)
            {
                voucherUpdate = _mapper.Map(voucherDTO, voucherUpdate);
                await _unitOfWork.VouchersRepository.UpdateAsync(voucherUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = true,
                        Message = "Vouchers update successfully!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Vouchers update failed!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Vouchers not found!"
            };
        }
    }
}
