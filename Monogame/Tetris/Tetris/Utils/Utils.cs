using System;

using Microsoft.Xna.Framework;

namespace Tetris {
    class Utils {
        private static Random random = new Random();

        public static Color getHighSaturatedRandomColor() {
            int[] rgb = new int[3];
            rgb[0] = random.Next(256);  // red
            rgb[1] = random.Next(256);  // green
            rgb[2] = random.Next(256);  // blue

            int max, min;

            if (rgb[0] > rgb[1]) {
                max = (rgb[0] > rgb[2]) ? 0 : 2;
                min = (rgb[1] < rgb[2]) ? 1 : 2;
            } else {
                max = (rgb[1] > rgb[2]) ? 1 : 2;
                int notmax = 1 + max % 2;
                min = (rgb[0] < rgb[notmax]) ? 0 : notmax;
            }

            rgb[max] = 255;
            rgb[min] = 0;

            return new Color(rgb[0], rgb[1], rgb[2]);
        }

        public static int GetRotatedIndex(int rotation, int sizeX, int x, int y) {
            int rotatedIndex = 0;

            switch (rotation % 360) {
                case 0:
                    rotatedIndex = y * sizeX + x;
                    break;

                case 90:
                    rotatedIndex = (sizeX * 3) + y - (x * sizeX);
                    break;

                case 180:
                    rotatedIndex = (sizeX * 4 - 1) - (sizeX * y) - x;
                    break;

                case 270:
                    rotatedIndex = (sizeX - 1) - y + (sizeX * x);
                    break;
            }

            return rotatedIndex;
        }
    }
}
