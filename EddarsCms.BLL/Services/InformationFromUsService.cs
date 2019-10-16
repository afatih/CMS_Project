﻿using AutoMapper;
using Core.DAL;
using Core.Results;
using EddarsCms.BLL.IServices;
using EddarsCms.DAL;
using EddarsCms.Dto.BasicDtos;
using EddarsCms.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EddarsCms.BLL.Services
{
    public class InformationFromUsService : IInformationFromUsService
    {

        IRepository<InformationFromUs> infoRepo;
        IUnitOfWork uow;

        public InformationFromUsService()
        {
            infoRepo = Resource.UoW.GetRepository<InformationFromUs>();
            uow = Resource.UoW;
        }


        public ServiceResult Delete(int id)
        {
            infoRepo.HardDelete(p => p.Id == id);
            var result = uow.Save();
            return result;
        }

        public ServiceResult<List<InformationFromUsDto>> GetAll()
        {
            try
            {
                var result = DtoFromEntity(infoRepo.Get(p => p.Id > 0));
                return new ServiceResult<List<InformationFromUsDto>>(ProcessStateEnum.Success, "İşmeniniz başarılı", result.OrderByDescending(x => x.Id).ToList());
            }
            catch (Exception e)
            {
                return new ServiceResult<List<InformationFromUsDto>>(ProcessStateEnum.Success, e.Message, new List<InformationFromUsDto>());
            }
        }

        #region Mappings
        public InformationFromUs EntityFromDto(InformationFromUsDto dto)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InformationFromUsDto, InformationFromUs>();
            });

            IMapper iMapper = config.CreateMapper();
            var entity = iMapper.Map<InformationFromUsDto, InformationFromUs>(dto);
            return entity;

        }

        public InformationFromUsDto DtoFromEntity(InformationFromUs entity)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<InformationFromUs, InformationFromUsDto>();
            });

            IMapper iMapper = config.CreateMapper();
            var dto = iMapper.Map<InformationFromUs, InformationFromUsDto>(entity);
            return dto;
        }

        public List<InformationFromUsDto> DtoFromEntity(List<InformationFromUs> dtos)
        {
            List<InformationFromUsDto> list = new List<InformationFromUsDto>();
            if (dtos != null)
            {
                if (dtos.Count > 0)
                {
                    foreach (var dto in dtos)
                    {
                        list.Add(DtoFromEntity(dto));
                    }
                }
            }
            return list;
        }


        #endregion
    }
}
