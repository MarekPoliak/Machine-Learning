using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dataAnalysis
{
    public static class CropImage
    {
        public static byte[,] Crop(byte[,] image)
        {
            image=CropRight(image);
            image=CropBottom(image);
            return image;
        }

        private static int RightOffset(byte[,] image)
        {
            int length = (int)Math.Sqrt(image.Length);
            int RightOffset = 0;
            for (int i = 0; i < length; i++)
            {
                int counter = length-1;
                while (counter != 0 && image[i, counter] != 255)
                {
                    counter--;
                }
                if (counter > RightOffset)
                {
                    RightOffset = counter;
                }
            }
            return length-RightOffset-1;
        }

        private static int LeftOffset(byte[,] image)
        {
            int length = (int)Math.Sqrt(image.Length);
            int LeftOffset = length;
            for (int i = 0; i < length; i++)
            {
                int counter = 0;
                while (counter != length && image[i, counter] != 255)
                {
                    counter++;
                }
                if (counter < LeftOffset)
                {
                    LeftOffset = counter;
                }
            }
            return LeftOffset;
        }

        private static int TopOffset(byte[,] image)
        {
            int length = (int)Math.Sqrt(image.Length);
            int TopOffset = length;
            for (int i = 0; i < length; i++)
            {
                int counter = 0;
                while (counter != length && image[counter,i] != 255)
                {
                    counter++;
                }
                if (counter < TopOffset)
                {
                    TopOffset = counter;
                }
            }
            return TopOffset;
        }

        private static int BottomOffset(byte[,] image)
        {
            int length = (int)Math.Sqrt(image.Length);
            int BottomOffset = 0;
            for (int i = 0; i < length; i++)
            {
                int counter = length - 1;
                while (counter != 0 && image[counter,i] != 255)
                {
                    counter--;
                }
                if (counter > BottomOffset)
                {
                    BottomOffset = counter;
                }
            }
            return length - BottomOffset-1;
        }

        private static byte[,] CropRight(byte[,] image)
        {
            image = TranslateLeft(image);
            int length = (int)Math.Sqrt(image.Length);
            var croppedRightImg = new byte[length, length];
            double widthRatio = length / (double)(length - RightOffset(image) - LeftOffset(image));
            for (int i = 0; i < length; i++)
            {
                int index = -1;
                int croppedIndex = 0;
                while (croppedIndex != length)
                {
                    int toBeCropped = 0;
                    do
                    {
                        toBeCropped++;
                        index++;
                    }while (index+2<length- RightOffset(image) && croppedIndex + 1 < length && image[i, index] == image[i, index + 1]);
                    for (int counter = 0; counter < Math.Round(toBeCropped*widthRatio); counter++)
                    {
                        croppedRightImg[i, croppedIndex] = image[i, index];
                        croppedIndex++;
                        if (croppedIndex == length)
                        {
                            break;
                        }
                    }
                }
            }
            return croppedRightImg;
        }

        private static byte[,] TranslateLeft(byte[,] image)
        {
            int length = (int)Math.Sqrt(image.Length);
            var translatedImg = new byte[length, length];
            int leftOffset = LeftOffset(image);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length-leftOffset; j++)
                {
                    translatedImg[i, j] = image[i, j+leftOffset];
                }
            }
            return translatedImg;
        }

        private static byte[,] CropBottom(byte[,] image)
        {
            image = TranslateTop(image);
            int length = (int)Math.Sqrt(image.Length);
            var croppedBottomImg = new byte[length, length];
            double heightRatio = length / (double)(length - TopOffset(image) - BottomOffset(image));
            for (int i = 0; i < length; i++)
            {
                int index = -1;
                int croppedIndex = 0;
                while (croppedIndex != length)
                {
                    int toBeCropped = 0;
                    do
                    {
                        toBeCropped++;
                        index++;
                    } while (index + 2 < length - BottomOffset(image) && croppedIndex + 1 != length && image[index,i] == image[index + 1,i]);
                    for (int counter = 0; counter < Math.Round(toBeCropped * heightRatio); counter++)
                    {
                        croppedBottomImg[croppedIndex,i] = image[index,i];
                        croppedIndex++;
                        if (croppedIndex == length)
                        {
                            break;
                        }
                    }
                }
            }
            return croppedBottomImg;
        }

        private static byte[,] TranslateTop(byte[,] image)
        {
            int length = (int)Math.Sqrt(image.Length);
            var translatedImg = new byte[length, length];
            int topOffset = TopOffset(image);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length - topOffset; j++)
                {
                    translatedImg[j,i] = image[j+topOffset,i];
                }
            }
            return translatedImg;
        }
    }
}
