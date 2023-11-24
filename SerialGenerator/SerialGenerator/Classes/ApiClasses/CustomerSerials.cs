using SerialGenerator.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using System.Security.Claims;
using Newtonsoft.Json.Converters;

namespace SerialGenerator.ApiClasses
{
    public class CustomerSerials
    {
        public int customerSerialId { get; set; }
        public string serial { get; set; }
        public string officeName { get; set; }
        public string Number { get; set; }
        public string customerHardCode { get; set; }
        public string activateCode { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> expireDate { get; set; }
        public int yearCount { get; set; }
        public Nullable<System.DateTime> bookDate { get; set; }
        public string confirmStat { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> isActive { get; set; }
        public string type { get; set; }
        public Nullable<System.DateTime> activatedate { get; set; }
        public bool isProgramActivated { get; set; }
        public bool canDelete { get; set; }
        public string strstartDate { get; set; }
        public string strexpireDate { get; set; }

        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<List<CustomerSerials>> GetAll()
        {

            List<CustomerSerials> List = new List<CustomerSerials>();
            bool canDelete = false;
            try
            {
                using (bookdbEntities entity = new bookdbEntities())
                {
                    List = (from S in entity.customerserials
                            select new CustomerSerials()
                            {
                                customerSerialId = S.customerSerialId,
                                serial = S.serial,
                                officeName = S.officeName,
                                Number = S.Number,
                                customerHardCode = S.customerHardCode,
                                activateCode = S.activateCode,
                                startDate = S.startDate,
                                expireDate = S.expireDate,
                                yearCount = S.yearCount,
                                bookDate = S.bookDate,
                                confirmStat = S.confirmStat,
                                notes = S.notes,
                                createDate = S.createDate,
                                updateDate = S.updateDate,
                                createUserId = S.createUserId,
                                updateUserId = S.updateUserId,
                                isActive = S.isActive,
                                type = S.type,
                                activatedate = S.activatedate,
                                isProgramActivated = S.isProgramActivated,
                                canDelete=true,
                            }).ToList();

                   
                    return List;
                }

            }
            catch
            {
                return List;
            }
        }

        public async Task<decimal> Save(CustomerSerials newitem)
        {
            customerserials newObject = new customerserials();
            newObject = JsonConvert.DeserializeObject<customerserials>(JsonConvert.SerializeObject(newitem));

            decimal message = 0;
            if (newObject != null)
            {
                if (newObject.updateUserId == 0 || newObject.updateUserId == null)
                {
                    Nullable<int> id = null;
                    newObject.updateUserId = id;
                }
                if (newObject.createUserId == 0 || newObject.createUserId == null)
                {
                    Nullable<int> id = null;
                    newObject.createUserId = id;
                }
                try
                {
                    using (bookdbEntities entity = new bookdbEntities())
                    {
                        var locationEntity = entity.Set<customerserials>();
                        if (newObject.customerSerialId == 0)
                        {
                            newObject.Number = generateCodeNumber();
                            newObject.createDate = DateTime.Now;
                            newObject.updateDate = newObject.createDate;
                            newObject.updateUserId = newObject.createUserId;
                            newObject.isActive = 1;
                            locationEntity.Add(newObject);
                            entity.SaveChanges();
                            message = newObject.customerSerialId;
                            //OfficeSystem osmodel = new OfficeSystem();
                            //decimal res = await osmodel.AddByOfficeId(newObject.customerSerialId);
                        }
                        else
                        {
                            var tmpObject = entity.customerserials.Where(p => p.customerSerialId == newObject.customerSerialId).FirstOrDefault();
                            // update is active in os list
                            //OfficeSystem osmodel = new OfficeSystem();
                            //int res = await osmodel.updateListisActive(tmpObject.customerSerialId,(bool)newObject.isActive, "customerserials");

                            tmpObject.updateDate = DateTime.Now;

                            tmpObject.customerSerialId = newObject.customerSerialId;
                            tmpObject.serial = newObject.serial;
                            tmpObject.officeName = newObject.officeName;
                            tmpObject.Number = newObject.Number;
                            tmpObject.customerHardCode = newObject.customerHardCode;
                            tmpObject.activateCode = newObject.activateCode;
                            tmpObject.startDate = newObject.startDate;
                            tmpObject.expireDate = newObject.expireDate;
                            tmpObject.yearCount = newObject.yearCount;
                            tmpObject.bookDate = newObject.bookDate;
                            tmpObject.confirmStat = newObject.confirmStat;
                            tmpObject.notes = newObject.notes;
                             
                            
                         
                            tmpObject.updateUserId = newObject.updateUserId;
                            tmpObject.isActive = newObject.isActive;
                            tmpObject.type = newObject.type;
                            tmpObject.activatedate = newObject.activatedate;
                            tmpObject.isProgramActivated = newObject.isProgramActivated;

                            entity.SaveChanges();

                            message = tmpObject.customerSerialId;
                        }
                    }
                    return message;
                }
                catch
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public async Task<CustomerSerials> GetByID(int itemId)
        {


            CustomerSerials item = new CustomerSerials();
           

            CustomerSerials row = new CustomerSerials();
            try
            {
                using (bookdbEntities entity = new bookdbEntities())
                {
                    var list = entity.customerserials.ToList();
                    row = list.Where(u => u.customerSerialId == itemId)
                     .Select(S => new CustomerSerials()
                     {
                         customerSerialId = S.customerSerialId,
                         serial = S.serial,
                         officeName = S.officeName,
                         Number = S.Number,
                         customerHardCode = S.customerHardCode,
                         activateCode = S.activateCode,
                         startDate = S.startDate,
                         expireDate = S.expireDate,
                         yearCount = S.yearCount,
                         bookDate = S.bookDate,
                         confirmStat = S.confirmStat,
                         notes = S.notes,
                         createDate = S.createDate,
                         updateDate = S.updateDate,
                         createUserId = S.createUserId,
                         updateUserId = S.updateUserId,
                         isActive = S.isActive,
                         type = S.type,
                         activatedate = S.activatedate,
                         isProgramActivated = S.isProgramActivated,

                     }).FirstOrDefault();
                    return row;
                }

            }
            catch (Exception ex)
            {
                row = new CustomerSerials();
                //userrow.name = ex.ToString();
                return row;
            }
        }
        public async Task<decimal> Delete(int id, int signuserId, bool final)
        {

            decimal message = 0;
            if (final)
            {
                try
                {
                    using (bookdbEntities entity = new bookdbEntities())
                    {
                        customerserials objectDelete = entity.customerserials.Find(id);
                       
                        //delete related OfficeSystem
                        //OfficeSystem OfficeSystemModel = new OfficeSystem();
                        //decimal res=   await OfficeSystemModel.DeletebyOfficeId(id);

                        entity.customerserials.Remove(objectDelete);
                        message = entity.SaveChanges();
                        return message;

                    }
                }
                catch
                {
                    return 0;

                }
            }
            else
            {
                try
                {
                    //OfficeSystem osmodel = new OfficeSystem();
                    using (bookdbEntities entity = new bookdbEntities())
                    {
                        customerserials objectDelete = entity.customerserials.Find(id);
                        //int res = await osmodel.updateListisActive(id, false, "customerserials");
                        objectDelete.isActive = 0;
                        objectDelete.updateUserId = signuserId;
                        objectDelete.updateDate = DateTime.Now;
                        message = entity.SaveChanges();

                        return message;
                    }
                }
                catch
                {
                    return 0;
                }
            }


        }

        //public async Task<string> generateCodeNumber(string type)
        //{
        //    int sequence = await GetLastNumOfCode(type);
        //    sequence++;
        //    string strSeq = sequence.ToString();
        //    if (sequence <= 999999)
        //        strSeq = sequence.ToString().PadLeft(6, '0');
        //    string transNum = type.ToUpper() + "-" + strSeq;
        //    return transNum;
        //}

        //public async Task<int> GetLastNumOfCode(string type)
        //{

        //    try
        //    {
        //        List<string> numberList;
        //        int lastNum = 0;
        //        using (bookdbEntities entity = new bookdbEntities())
        //        {
        //            numberList = entity.customerserials.Where(b => b.nu.Contains(type + "-")).Select(b => b.serviceNum).ToList();

        //            for (int i = 0; i < numberList.Count; i++)
        //            {
        //                string code = numberList[i];
        //                string s = code.Substring(code.LastIndexOf("-") + 1);
        //                numberList[i] = s;
        //            }
        //            if (numberList.Count > 0)
        //            {
        //                numberList.Sort();
        //                lastNum = int.Parse(numberList[numberList.Count - 1]);
        //            }
        //        }

        //        return lastNum;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //}
        public string generateCodeNumber()
        {
            int sequence = GetLastNumOfCode();
            sequence++;
            string strSeq = sequence.ToString();
            //if (sequence <= 999999)
            //    strSeq = sequence.ToString().PadLeft(6, '0');
            //string transNum = type.ToUpper() + "-" + strSeq;
            return strSeq;
        }
        public int GetLastNumOfCode()
        {

            try
            {
                List<string> numberList;
                int lastNum = 0;
                using (bookdbEntities entity = new bookdbEntities())
                {
                    numberList = entity.customerserials.Select(b => b.Number).ToList();

                    //for (int i = 0; i < numberList.Count; i++)
                    //{
                    //    string code = numberList[i];
                    //    string s = code.Substring(code.LastIndexOf("-") + 1);
                    //    numberList[i] = s;
                    //}
                    if (numberList.Count > 0)
                    {
                        numberList.Sort();
                        lastNum = int.Parse(numberList[numberList.Count - 1]);
                    }
                }

                return lastNum;
            }
            catch
            {
                return 0;
            }
        }
    }
}
