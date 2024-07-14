using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.enums;
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
            vouchersObj.dateFrom = DateTime.Now;
            vouchersObj.dateTo = vouchersDTO.dateTo;
            vouchersObj.vouchersStatus = VouchersStatus.Active;
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
            var now = DateTime.Now;

            // Fetch all vouchers
            var vouchersGetAll = await _unitOfWork.VouchersRepository.GetAllAsync();

            // List to hold vouchers that need their status updated
            var vouchersToUpdate = new List<Vouchers>();

            foreach (var voucher in vouchersGetAll)
            {
                if (voucher.dateTo < now && voucher.vouchersStatus != VouchersStatus.Expired)
                {
                    // Voucher is expired and needs status update
                    voucher.vouchersStatus = VouchersStatus.Expired;
                    vouchersToUpdate.Add(voucher);
                }
            }

            // Update the status of expired vouchers
            if (vouchersToUpdate.Any())
            {
                _unitOfWork.VouchersRepository.UpdateRange(vouchersToUpdate);
                await _unitOfWork.SaveChangeAsync();
            }

            // Map the vouchers to VouchersDTO and return
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

        public async Task<ResponseDTO> UpdateVouchersAsync(int id, VouchersDTO vouchersDTO)
        {
            var voucherUpdate = await _unitOfWork.VouchersRepository.GetByIdAsync(id);
            if (voucherUpdate != null)
            {
                voucherUpdate = _mapper.Map(vouchersDTO, voucherUpdate);
                await _unitOfWork.VouchersRepository.UpdateAsync(voucherUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = false,
                        Message = "Vouchers update failed!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Vouchers update successfully!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Vouchers not found!"
            };
        }

        public async Task<IEnumerable<VouchersDTO>> GetActiveVouchersAsync()
        {
            var now = DateTime.UtcNow;
            var vouchers = await _unitOfWork.VouchersRepository.GetVouchersByStatusAsync(VouchersStatus.Active);
            return _mapper.Map<IEnumerable<VouchersDTO>>(vouchers);
        }
    }
}
