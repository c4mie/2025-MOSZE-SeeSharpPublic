using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BasicPlaymodeTests
{
    [UnityTest]
    public IEnumerator SmokeTest_FrameRuns()
    {
        yield return null;

        Assert.Pass();
    }
}
