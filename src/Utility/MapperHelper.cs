using System;
using System.Collections.Generic;
using AutoMapper;

namespace dotNET.Core
{

    /// <summary>
    /// 
    /// </summary>
    public class MapperHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Tsou">源</typeparam>
        /// <typeparam name="Tdes">目标</typeparam>
        /// <param name="sou">源</param>
        /// <returns></returns>
        public static Tdes Map<Tsou, Tdes>(Tsou sou)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Tsou, Tdes>());
            return Mapper.Map<Tsou, Tdes>(sou);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Tsou">源</typeparam>
        /// <typeparam name="Tdes">目标</typeparam>
        /// <param name="sou">源</param>
        /// <param name="des">目标</param>
        /// <returns></returns>
        public static Tdes Map<Tsou, Tdes>(Tsou sou, Tdes des)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Tsou, Tdes>());
            return Mapper.Map<Tsou, Tdes>(sou, des);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="Tsou"></typeparam>
        /// <typeparam name="Tdes"></typeparam>
        /// <param name="sou"></param>
        /// <returns></returns>
        public static List<Tdes> MapList<Tsou, Tdes>(List<Tsou> sou)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Tsou, Tdes>());
            return Mapper.Map<List<Tsou>, List<Tdes>>(sou);
        }
    }
}
