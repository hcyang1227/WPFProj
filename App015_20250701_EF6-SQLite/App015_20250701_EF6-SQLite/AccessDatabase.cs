using System.Windows;
using System.Linq;
using System.Data.Entity;

namespace App015_20250701_EF6_SQLite
{
    public class AccessDatabase
    {
        public static void InitialProcess()
        {
            using (var db = new mainEntitiesDingZing())
            {
                // 秀出Material目前資料筆數
                int materialCount = db.Materials.Count();
                MessageBox.Show($"Material 表格目前有 {materialCount} 筆資料");

                // 建立新材料紀錄
                if (materialCount <= 0)
                {
                    var newMaterial = new Material
                    {
                        index = 1,
                        roll_width = 1880
                    };
                    db.Materials.Add(newMaterial);
                    db.SaveChanges();
                }

                // 查詢該材料底下的影像與缺陷
                var mat = db.Materials
                            .Include(m => m.Images.Select(i => i.Defects)) // 載入關聯
                            .FirstOrDefault(m => m.index == 1);

                foreach (var img in mat.Images)
                {
                    MessageBox.Show($"Image: {img.path}, Time: {img.datetime}");
                    foreach (var defect in img.Defects)
                    {
                        MessageBox.Show($"↳ Defect: {defect.kind}, X: {defect.roll_x}, Y: {defect.roll_y}");
                    }
                }
            }
        }
    }
}
