using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace NoMissingGeneIcons;

[StaticConstructorOnStartup]
public static class Main
{
    private static readonly List<string> genePrefixes = ["AddictionResistant", "AddictionImmune", "ChemicalDependency"];

    static Main()
    {
        if (!ModLister.BiotechInstalled)
        {
            LogMessage("Biotech not active, nothing to do.");
            return;
        }

        if (ModLister.GetActiveModWithIdentifier("RedMattis.GeneExtractor", true) != null)
        {
            genePrefixes.Add("GET_RegularAddiction");
        }

        var cachedIconField = AccessTools.Field(typeof(GeneDef), "cachedIcon");
        var fixedChemicals = new HashSet<string>();
        foreach (var chemicalDef in DefDatabase<ChemicalDef>.AllDefsListForReading)
        {
            foreach (var genePrefix in genePrefixes)
            {
                var currentGeneDef = DefDatabase<GeneDef>.GetNamedSilentFail($"{genePrefix}_{chemicalDef.defName}");
                if (currentGeneDef == null)
                {
                    continue;
                }

                if (ContentFinder<Texture2D>.Get(currentGeneDef.iconPath, false) != null)
                {
                    continue;
                }

                cachedIconField.SetValue(currentGeneDef, generateChemicalIcon(chemicalDef, genePrefix));
                fixedChemicals.Add(chemicalDef.LabelCap);
            }
        }

        LogMessage(
            $"Fixed missing gene-graphics for {fixedChemicals.Count} ChemicalDefs. \n{string.Join(", ", fixedChemicals)}");
    }

    private static Texture2D generateChemicalIcon(ChemicalDef chemicalDef, string genePrefix)
    {
        var templateImage = ContentFinder<Texture2D>.Get(genePrefix == "GET_RegularAddiction"
            ? "UI/Icons/Genes/GeneBackground_Xenogene"
            : $"GeneTextureTemplates/{genePrefix}");

        var firstThingUsingChemical = DefDatabase<ThingDef>.AllDefsListForReading.FirstOrFallback(
            def => def.HasComp(typeof(CompDrug)) &&
                   def.GetCompProperties<CompProperties_Drug>().chemical == chemicalDef);

        Texture2D thingIcon;
        if (firstThingUsingChemical == null)
        {
            LogMessage($"Failed to find anything using {chemicalDef.label}, using default icon.");
            thingIcon = ContentFinder<Texture2D>.Get("GeneTextureTemplates/Generic");
        }
        else
        {
            thingIcon = Widgets.GetIconFor(firstThingUsingChemical);
        }


        return combineTextures(getReadableTexture(templateImage), getReadableTexture(thingIcon));
    }

    private static Texture2D combineTextures(Texture2D textureA, Texture2D textureB)
    {
        var textureResult = new Texture2D(textureA.width, textureA.height);
        if (textureB.width != textureA.width / 2 || textureB.height != textureA.height / 2)
        {
            textureB = ResizeTool.Resize(textureB, textureA.width / 2, textureA.height / 2);
        }

        textureResult.SetPixels(textureA.GetPixels());
        for (var x = 0; x < textureB.width; x++)
        {
            for (var y = 0; y < textureB.height; y++)
            {
                var c = textureB.GetPixel(x, y);
                if (c.a > 0.0f)
                {
                    textureResult.SetPixel(x + (textureA.width / 4), y + (textureA.height / 4), c);
                }
            }
        }

        textureResult.Apply();
        return textureResult;
    }

    private static Texture2D getReadableTexture(Texture2D texture)
    {
        var renderTexture = RenderTexture.GetTemporary(
            texture.width,
            texture.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);
        Graphics.Blit(texture, renderTexture);
        var previous = RenderTexture.active;
        RenderTexture.active = renderTexture;
        var tex = new Texture2D(texture.width, texture.height);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTexture);
        return tex;
    }


    public static void LogMessage(string message)
    {
        Log.Message($"[NoMissingGeneIcons]: {message}");
    }
}