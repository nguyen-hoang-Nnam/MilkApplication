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

namespace MilkApplication.BLL.Service
{
    public class ComboService : IComboService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ComboService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> AddComboAsync(ComboDTO comboDTO)
        {
            var comboObj = _mapper.Map<Combo>(comboDTO);
            await _unitOfWork.ComboRepository.AddAsync(comboObj);
            await _unitOfWork.SaveChangeAsync();

            var response = new ResponseDTO
            {
                IsSucceed = true,
                Message = "Combo added successfully",
            };
            return response;
        }

        public async Task<ResponseDTO> DeleteComboAsync(int id)
        {
            var deleteCombo = await _unitOfWork.ComboRepository.GetByIdAsync(id);
            if (deleteCombo != null)
            {
                await _unitOfWork.ComboRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Combo deleted successfully"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = $"Combo with ID {id} not found"
                };
            }
        }

        public async Task<List<ComboDTO>> GetAllCombosAsync()
        {
            var comboGetAll = await _unitOfWork.ComboRepository.GetAllAsync();
            var comboMapper = _mapper.Map<List<ComboDTO>>(comboGetAll);
            return comboMapper;
        }

        public async Task<ComboDTO> GetComboByIdAsync(int id)
        {
            var comboFound = await _unitOfWork.ComboRepository.GetByIdAsync(id);
            if (comboFound == null)
            {
                return null;
            }
            var comboMapper = _mapper.Map<ComboDTO>(comboFound);
            return comboMapper;
        }

        public async Task<ResponseDTO> UpdateComboAsync(int id, ComboDTO comboDTO)
        {
            var comboUpdate = await _unitOfWork.ComboRepository.GetByIdAsync(id);
            if (comboUpdate != null)
            {
                comboUpdate = _mapper.Map(comboDTO, comboUpdate);
                await _unitOfWork.ComboRepository.UpdateAsync(comboUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = true,
                        Message = "Combo update successfully!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Combo update failed!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Combo not found!"
            };
        }
    }
}
