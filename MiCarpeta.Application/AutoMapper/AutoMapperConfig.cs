using AutoMapper;
using MiCarpeta.Common;
using MiCarpeta.Domain.Entities;

namespace MiCarpeta.Application.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static readonly object ThisLock = new object();

        public static void Initialize()
        {
            // This will ensure one thread can access to this static initialize call
            // and ensure the mapper is reseted before initialized
            lock (ThisLock)
            {
                Mapper.Reset();
                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Response, ResponseViewModel>()
                        .ReverseMap();
                });

            }
        }
    }
}
