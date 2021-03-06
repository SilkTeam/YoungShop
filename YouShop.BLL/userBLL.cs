﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YouShop.DAL;
using YouShop.Model;

namespace YouShop.BLL
{
    public class UserBLL
    {
        /// <summary>
        /// 初始化EF
        /// </summary>
        private YoungShopEntites _ef;
        private YoungShopEntites EF
        {
            get
            {
                if (_ef == null)
                    _ef = new YoungShopEntites();
                return _ef;
            }
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sigin"></param>
        /// <returns></returns>
        public Sigin GetSign(Sigin sigin)
        {
            return EF.Sigins.FirstOrDefault(x => x.Account == sigin.Account && x.Password == sigin.Password);
        }
        /// <summary>
        /// 查找用户是否存在
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public bool FindUser(string Account)
        {
            return EF.Sigins.FirstOrDefault(x => x.Account == Account) != null;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sigin"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool GetReg(Sigin sigin, User user)
        {
            var member = new Sigin()
            {
                Account = sigin.Account,
                Password = sigin.Password,
                Identity = sigin.Identity,
                Users = new List<User>()
                {
                    new User()
                    {
                        Name = user.Name,
                        Img = null,
                        Sex = user.Sex,
                        Age = user.Age,
                        Phone = user.Phone,
                        Email = user.Email,
                        WalletID = null,
                        EntryTime = DateTime.Now,
                    },
                },
            };
            EF.Sigins.Add(member);
            return EF.SaveChanges() > 0;
        }
        /// <summary>
        /// 编辑信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool EditInfo(User user)
        {
            User mod = EF.Users.FirstOrDefault(x => x.ID == user.ID);
            mod.Name = user.Name;
            mod.Sex = user.Sex;
            mod.Age = user.Age;
            mod.Email = user.Email;
            mod.Phone = user.Phone;
            return EF.SaveChanges() > 0;
        }
        /// <summary>
        /// 找回密码
        /// </summary>
        /// <param name="email"></param>
        /// <param name="sigin"></param>
        /// <returns></returns>
        public bool RestPWD(string Email, Sigin sigin)
        {
            var mod = EF.Sigins.FirstOrDefault(x => x.Account == sigin.Account);
            if (mod != null)
            {
                var user = EF.Users.FirstOrDefault(x => x.SiginID == mod.ID);
                if (user.Email == Email)
                {
                    mod.Password = sigin.Password;
                }
            }
            return EF.SaveChanges() > 0;
        }
        public User GetInfo(int ID)
        {
            return EF.Users.Include("Sigin").FirstOrDefault(x => x.SiginID == ID);
        }
        //find wallet
        public User Wallet(int ID)
        {
            return EF.Users.Include("Wallet").FirstOrDefault(x => x.SiginID == ID);
        }
        public bool GoWallet(int ID)
        {
            Wallet wallet = new Wallet()
            {
                Money = 1.68,
            };
            EF.Wallets.Add(wallet);
            EF.SaveChanges();
            var mod = EF.Users.FirstOrDefault(x => x.SiginID == ID);
            mod.WalletID = 1;
            return EF.SaveChanges() > 0;
        }
        public bool ccc(Wallet wallet)
        {
            var mod = EF.Wallets.FirstOrDefault(x => x.ID == wallet.ID);
            mod.Money += wallet.Money;
            return EF.SaveChanges() > 0;
        }
    }
}