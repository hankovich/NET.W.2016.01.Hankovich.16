using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Task1.Logic
{
    public class ShawarmaOperations
    {
         /* 
            +Добавления продуктов на склад.
            +Продажа шаурмы (уменьшение количества продуктов на складе).
            +Создание нового рецепта шаурмы.
            +Установка новой цены.
            +Добавление новой торговой точки.
            Добавление нового продавца-повара.
            Составление отчета по продажам конкретной торговой точки за указанный  временной интервал.
            Составление отчета по каждому продавцу, заработная плата которого складывается из суммарного времени нахождения в торговой точке и времени, потраченного на приготовление шаурмы, за указанный  временной интервал.
        */
        public static bool AddIngredient(string name, string categoryName, int weight)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (categoryName == null) throw new ArgumentNullException(nameof(categoryName));
            if (weight <= 0) throw new ArgumentOutOfRangeException(nameof(weight));

            using (var sb = new ShawarmaBaseEntities())
            {
                Ingradient ingradient = sb.Ingradient.FirstOrDefault(ingr => ingr.IngradientName == name);
                if (ingradient != null)
                    ingradient.TotalWeight += weight;
                else
                {
                    IngradientCategory category = sb.IngradientCategory.FirstOrDefault(ingr => ingr.CategoryName == categoryName);
                    if (category == null)
                        return false;
                    int categoryId = category.CategoryId;
                    sb.Ingradient.Add(new Ingradient
                    {
                        CategoryId = categoryId,
                        IngradientName = name,
                        TotalWeight = weight
                    });
                }
                return SaveIt(sb);
            }
        }

        public static bool SellSomeShawarma(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            using (var sb = new ShawarmaBaseEntities())
            {
                Shawarma shawarma = sb.Shawarma.FirstOrDefault(shaw => shaw.ShawarmaName == name);

                if (shawarma == null)
                    return false;

                foreach (var ingradient in shawarma.ShawarmaRecipe)
                {
                    if (ingradient.Weight > ingradient.Ingradient.TotalWeight)
                        return false;
                    
                    ingradient.Ingradient.TotalWeight -= ingradient.Weight;
                }
                return SaveIt(sb);
            }
        }

        public static bool CreateRecipe(string name, string[] ingradients, int[] weights, int cookingTime)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (ingradients == null || weights == null)
                throw new ArgumentNullException(nameof(ingradients));
            if (ingradients.Length == 0 || weights.Length == 0 || ingradients.Length != weights.Length)
                throw new ArgumentException();

            using (var sb = new ShawarmaBaseEntities())
            {
                if (!sb.Shawarma.Any(recipe => recipe.ShawarmaName == name))
                    return false;

                Shawarma shawarma = new Shawarma
                {
                    ShawarmaName = name,
                    CookingTime = cookingTime
                };

                sb.Shawarma.Add(shawarma);

                if (sb.Ingradient.Count(ingr => ingradients.Contains(ingr.IngradientName)) != ingradients.Length)
                    return false;

                var ingradientsInBase = sb.Ingradient.Where(ingr => ingradients.Contains(ingr.IngradientName));
                
                for (int i = 0; i < ingradients.Length; i++)
                {
                    var sr = new ShawarmaRecipe
                    {
                        IngradientId = ingradientsInBase.First(ing => ing.IngradientName == ingradients[i]).IngradientId,
                        ShawarmaId = shawarma.ShawarmaId,
                        Weight = weights[i]
                    };
                    sb.ShawarmaRecipe.Add(sr);
                }
                return SaveIt(sb);
            }
        }

        public static bool SetNewPrice(string name, decimal price, string comment, string sellingPointTitle)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));
            if (sellingPointTitle == null)
                throw new ArgumentNullException(nameof(sellingPointTitle));
            if (price <= 0)
                throw new ArgumentOutOfRangeException(nameof(price));

            using (var sb = new ShawarmaBaseEntities())
            {
                Shawarma shawarma = sb.Shawarma.FirstOrDefault(sh => sh.ShawarmaName == name);
                if (shawarma == null)
                    return false;

                SellingPoint sp = sb.SellingPoint.FirstOrDefault(s => s.ShawarmaTitle == sellingPointTitle);
                if (sp == null)
                    return false;

                PriceController pc = sb.PriceController.FirstOrDefault(p => p.ShawarmaId == shawarma.ShawarmaId&& p.SellingPointId == sp.SellingPointId);
                if (pc == null)
                {
                    pc = new PriceController
                    {
                        Price = price,
                        Comment = comment,
                        SellingPointId = sp.SellingPointId,
                        ShawarmaId = shawarma.ShawarmaId
                    };
                    sb.PriceController.Add(pc);
                }
                else
                {
                    pc.Price = price;
                    pc.Comment = comment;
                }

                return SaveIt(sb);
            }
        }

        public static bool AddSellingPoint(string title, string address, string category)
        {
            using (var sb = new ShawarmaBaseEntities())
            {
                var pointCat = sb.SellingPointCategory.FirstOrDefault(selling => selling.SellingPointCategoryName == category);
                if (pointCat == null)
                    return false;

                SellingPoint point = new SellingPoint
                {
                    ShawarmaTitle = title,
                    Address = address,
                    SellingPointCategoryId = pointCat.SellingPointCategoryId
                };

                sb.SellingPoint.Add(point);
                return SaveIt(sb);
            }
        }

        private static bool SaveIt(DbContext sb)
        {
            try
            {
                sb.SaveChanges();
                return true;
            }
            catch(DbUpdateException)
            {
                return false;
            }
        }
    }
}
