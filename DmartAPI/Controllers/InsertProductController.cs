using connection;
using DmartAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DmartAPI.Controllers
{
    public class InsertProductController : ApiController
    {
        Connect ob1 = new Connect();
        dmratEntities1 db = new dmratEntities1();


        // insert API
        // this will take all the detail of product transaction

        [Route("api/InsertProduct/InsertProduct")]
        [HttpPost]
        public IHttpActionResult InsertProduct([FromBody] purchaseProductTable[] data)
        {
            int len = data.Length;

            double TOTAL_AMOUNT = 0.0;

            purchase_table obj = new purchase_table();

            obj.purchase_date = DateTime.Now;
            obj.purchase_number = ob1.generateUniqueNumber();

            int PUR_ID;

            try
            {
                ob1.connect();

                for (int i = 0; i < len; i++)
                {
                    TOTAL_AMOUNT = TOTAL_AMOUNT + data[i].amount;
                }
                obj.total_amount = TOTAL_AMOUNT;

                db.purchase_table.Add(obj);
                db.SaveChanges();

                PUR_ID = obj.purchase_id;

                for (int i = 0; i < len; i++)
                {
                    data[i].purchase_id = PUR_ID;
                    db.purchaseProductTables.Add(data[i]);
                    db.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return (IHttpActionResult)ex;
            }
        }

        [Route("api/InsertProduct/DeleteProductByProductId")]
        [HttpDelete]
        public IHttpActionResult DeleteProductByProductId([FromBody] int data)
        {
            try
            {
                var temp = db.purchase_table.Where(t => t.purchase_id == data).FirstOrDefault();
                db.Entry(temp).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                db.purchaseProductTables.RemoveRange(db.purchaseProductTables.Where(t => t.purchase_id == data));
                db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return (IHttpActionResult)ex;
            }
        }

        [Route("api/InsertProduct/DeleteProductInpurchaseProductTable")]
        [HttpDelete]
        public IHttpActionResult DeleteProductInpurchaseProductTable([FromBody] int ID)
        {
            //int it_id = int.Parse(data[0]);
            try
            {
                var temp1 = db.purchaseProductTables.Where(t => t.purchase_product_id == ID).FirstOrDefault();
                //var temp2 = db.purchaseProductTables.Where(t => t.item_name.Equals(data[1])).FirstOrDefault();

                double d_amount = temp1.amount;

                //if(temp1!=null && temp2!=null)
                //{
                //    db.Entry(temp1).State = System.Data.Entity.EntityState.Deleted;
                //    db.SaveChanges();
                //}
                db.Entry(temp1).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();


                var temp = db.purchase_table.Where(t => t.purchase_id == temp1.purchase_id).FirstOrDefault();
                temp.total_amount -= d_amount;

                db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return (IHttpActionResult)ex;
            }
        }

        // data[0] = purchase_product_id
        // data[1] = qty
        // data[2] = updated amount
        [Route("api/InsertProduct/updateProductDetail")]
        [HttpPut]
        public IHttpActionResult updateProductDetail([FromBody] int[] data)
        {
            int pur_product_id = data[0];
            int QTY = data[1];
            double amount = data[2];

            try
            {
                if (data[1] <= 0)
                {
                    var temp1 = db.purchaseProductTables.Where(t => t.purchase_product_id == pur_product_id).FirstOrDefault();
                    db.Entry(temp1).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    var temp1 = db.purchaseProductTables.Where(t => t.purchase_product_id == pur_product_id).FirstOrDefault();

                    double d_amount = temp1.amount;

                    temp1.qty = QTY;
                    temp1.amount = amount;
                    db.SaveChanges();

                    var temp = db.purchase_table.Where(t => t.purchase_id == temp1.purchase_id).FirstOrDefault();
                    temp.total_amount -= d_amount;
                    temp.total_amount += amount;
                    db.SaveChanges();

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return (IHttpActionResult)ex;
            }
        }
    }
}
