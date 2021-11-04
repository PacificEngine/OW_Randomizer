using OWML.Common;
using OWML.ModHelper;
using OWML.ModHelper.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ClassLibrary2
{
    public class MakeSprite : MonoBehaviour
    {
        public Sprite sprite;

        void Start()
        {
            GameObject go = new GameObject("New Sprite");
            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
        }
    }

    static class EyeCoordinates
    {
        private static NomaiCoordinateInterface nomaiCoordinateInterface = null;
        private static ScreenPromptElement eyePromptElement = null;
        private static System.Random random = new System.Random();

        private static int[] _x = new int[] { 1, 5, 4 };
        private static int[] _y = new int[] { 3, 0, 1, 4 };
        private static int[] _z = new int[] { 1, 2, 3, 0, 5, 4 };

        private static int[] x
        {
            get
            {
                return nomaiCoordinateInterface?.GetValue<int[]>("_coordinateX") ?? _x;
            }
            set
            {
                _x = value;
                nomaiCoordinateInterface?.SetValue("_coordinateX", value);
            }
        }

        private static int[] y
        {
            get
            {
                return nomaiCoordinateInterface?.GetValue<int[]>("_coordinateY") ?? _y;
            }
            set
            {
                _y = value;
                nomaiCoordinateInterface?.SetValue("_coordinateY", value);
            }
        }

        private static int[] z
        {
            get
            {
                return nomaiCoordinateInterface?.GetValue<int[]>("_coordinateZ") ?? _z;
            }
            set
            {
                _z = value;
                nomaiCoordinateInterface?.SetValue("_coordinateZ", value);
            }
        }

        public static void Start()
        {
            Helper.helper.HarmonyHelper.AddPrefix<NomaiCoordinateInterface>("Awake", typeof(EyeCoordinates), "onNomaiCoordinateInterfaceAwake");
            Helper.helper.HarmonyHelper.AddPostfix<KeyInfoPromptController>("Start", typeof(EyeCoordinates), "onKeyInfoPromptControllerStart");
        }

        public static void Awake()
        {
        }

        public static void Destroy()
        {
        }


        public static void Update()
        {
        }

        public static void randomizeCoordinates()
        {
            setCoordinates(generateCoordinate(), generateCoordinate(), generateCoordinate());
        }

        public static void setCoordinates(int[] x, int[] y, int[] z)
        {
            EyeCoordinates.x = x;
            EyeCoordinates.y = y;
            EyeCoordinates.z = z;
            updateCoordinates();
        }

        public static void updateCoordinates()
        {
            if (eyePromptElement)
            {
                var children = eyePromptElement.GetValue<List<GameObject>>("_children");
                drawCoordinate(eyePromptElement.transform, children, getCoordinate(x), getCoordinate(y), getCoordinate(z));
            }
        }

        private static bool onNomaiCoordinateInterfaceAwake(ref NomaiCoordinateInterface __instance)
        {
            EyeCoordinates.nomaiCoordinateInterface = __instance;
            EyeCoordinates.setCoordinates(_x, _y, _z);
            return true;
        }

        private static void onKeyInfoPromptControllerStart(ref KeyInfoPromptController __instance)
        {
            var manager = Locator.GetPromptManager();
            var oldPrompt = __instance.GetValue<ScreenPrompt>("_eyeCoordinatesPrompt");
            var eyePrompt = new ScreenPrompt(UITextLibrary.GetString(UITextType.EyeCoordinates));
            manager.RemoveScreenPrompt(oldPrompt);
            eyePromptElement = manager.AddScreenPrompt(eyePrompt, manager.GetScreenPromptList(PromptPosition.LowerLeft), manager.GetTextAnchor(PromptPosition.LowerLeft));
            __instance.SetValue("_eyeCoordinatesPrompt", eyePrompt);
            EyeCoordinates.updateCoordinates();
        }

        private static void drawCoordinate(Transform parent, List<GameObject> children, Vector3[] x, Vector3[] y, Vector3[] z)
        {
            /*foreach (GameObject child in children)
            {
                child.transform.SetParent(null);
                child.SetActive(false);
                if (child.GetComponent<Image>())
                {
                    child.GetComponent<Image>().SetLayoutDirty();
                    child.GetComponent<Image>().sprite = null;
                }
                GameObject.Destroy(child);
            }
            children.Clear();
            var goX = drawCoordinate(parent, "EyeCoordsX", x, 0f);
            var goY = drawCoordinate(parent, "EyeCoordsY", y, 5f);
            var goZ = drawCoordinate(parent, "EyeCoordsZ", z, 10f);
            children.Add(goX);
            children.Add(goY);
            children.Add(goZ);*/
            eyePromptElement.GetPromptData().SetText(UITextLibrary.GetString(UITextType.EyeCoordinates) + "(" + string.Join(",", _x) + ") " + "(" + string.Join(",", _y) + ") " + "(" + string.Join(",", _z) + ")");
        }

        private static GameObject drawCoordinate(Transform parent, String name, Vector3[] w, float positionX)
        {
            Helper.helper.Console.WriteLine(string.Join(",", w));

            GameObject child = new GameObject(name, new System.Type[1] { typeof(RectTransform) });
            var image = child.AddComponent<Image>();
            image.SetLayoutDirty();
            image.preserveAspect = true;

            child.transform.SetParent(parent);
            child.SetActive(true);
            child.transform.localScale = Vector3.one;
            child.transform.localRotation = Quaternion.identity;
            child.GetRequiredComponent<RectTransform>().SetLocalPositionZ(0.0f);

            var canvas = image.canvas;
            var multiplier = 10f / 4f;
            var position = new Vector3(positionX, 0f, 10f);
            for (var i = 1; i < w.Length; i++)
            {
                var line = CreateLine(name + "-" + i, (w[i-1] + position) * multiplier, (w[i] + position) * multiplier);
                line.transform.SetParent(image.canvas.transform, false);
            }

            var _eyeCoordsImgLayoutElement = child.AddComponent<LayoutElement>();
            _eyeCoordsImgLayoutElement.preferredHeight = 10f;
            _eyeCoordsImgLayoutElement.preferredWidth = 10f;

            return child;
        }

        private static GameObject CreateLine(String name, Vector3 p1, Vector3 p2)
        {
            GameObject line = new GameObject(name, new System.Type[1] { typeof(RectTransform) });
            var linerenderer = line.AddComponent<LineRenderer>();
            linerenderer.SetPosition(0, p1);
            linerenderer.SetPosition(1, p2);
            linerenderer.startColor = Color.white;
            linerenderer.endColor = Color.white;
            linerenderer.startWidth = 1f;
            linerenderer.endWidth = 1f;
            linerenderer.enabled = true;

            return line;
        }

        private static Vector3[] getCoordinate(int[] coordinate)
        {
            var vectors = new Vector3[coordinate.Length];
            for (int i = 0; i < coordinate.Length; i++)
            {
                vectors[i] = getCoordinate(coordinate[i]);
            }
            return vectors;
        }

        private static Vector3 getCoordinate(int coordinate)
        {
            if (coordinate == 0)
            {
                return new Vector3(1f, 2f);
            }
            if (coordinate == 1)
            {
                return new Vector3(2f, 2f);
            }
            if (coordinate == 2)
            {
                return new Vector3(3f, 1f);
            }
            if (coordinate == 3)
            {
                return new Vector3(2f, 0f);
            }
            if (coordinate == 4)
            {
                return new Vector3(1f, 0f);
            }
            if (coordinate == 5)
            {
                return new Vector3(0f, 1f);
            }
            return new Vector3(2f, 1f);
        }

        private static int[] generateCoordinate()
        {
            var coodinate = random.Next(0, 63);
            var list = new List<int>();
            if ((coodinate & 0x1) != 0)
            {
                list.Add(1);
            }
            if ((coodinate & 0x2) != 0)
            {
                list.Add(2);
            }
            if ((coodinate & 0x4) != 0)
            {
                list.Add(3);
            }
            if ((coodinate & 0x8) != 0)
            {
                list.Add(4);
            }
            if ((coodinate & 0x10) != 0)
            {
                list.Add(5);
            }
            if ((coodinate & 0x20) != 0)
            {
                list.Add(0);
            }
            Shuffle(list);

            return list.ToArray();
        }

        private static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
