namespace Falsus.Providers.Internet.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Falsus.GeneratorProperties;
    using Falsus.Shared.Avatar.Enums;

    [TestClass]
    public class AvatarSvgProviderUnitTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorWithNoParametersThrowsException()
        {
            // Act
            new AvatarSvgProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRangedRowValueThrowsException()
        {
            // Arrange
            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            provider.GetRangedRowValue(null, null, Array.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetRowValueWithExcludedRangesThrowsException()
        {
            // Arrange
            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            provider.GetRowValue(default, Array.Empty<WeightedRange>(), Array.Empty<string>());
        }

        [TestMethod]
        public void GetRowValueWithIdThrowsException()
        {
            // Arrange
            var expected = "svg goes here";

            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            var actual = provider.GetRowValue(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetRowValueWithSeedReturnsExpectedValue()
        {
            // Arrange
            int seed = 552367;
            string expected = "<svg\r\n        width=\"264px\"\r\n        height=\"280px\"\r\n        viewBox=\"0 0 264 280\"\r\n        version=\"1.1\"\r\n        xmlns=\"http://www.w3.org/2000/svg\"\r\n        xmlns:xlink=\"http://www.w3.org/1999/xlink\">\r\n  <desc>Created with getavataaars.com</desc>\r\n  <defs>\r\n    <circle id=\"path-1\" cx=\"120\" cy=\"120\" r=\"120\" />\r\n    <path\r\n      d=\"M12,160 C12,226.27417 65.72583,280 132,280 C198.27417,280 252,226.27417 252,160 L264,160 L264,-1.42108547e-14 L-3.19744231e-14,-1.42108547e-14 L-3.19744231e-14,160 L12,160 Z\"\r\n      id=\"path-3\"\r\n          />\r\n    <path\r\n      d=\"M124,144.610951 L124,163 L128,163 L128,163 C167.764502,163 200,195.235498 200,235 L200,244 L0,244 L0,235 C-4.86974701e-15,195.235498 32.235498,163 72,163 L72,163 L76,163 L76,144.610951 C58.7626345,136.422372 46.3722246,119.687011 44.3051388,99.8812385 C38.4803105,99.0577866 34,94.0521096 34,88 L34,74 C34,68.0540074 38.3245733,63.1180731 44,62.1659169 L44,56 L44,56 C44,25.072054 69.072054,5.68137151e-15 100,0 L100,0 L100,0 C130.927946,-5.68137151e-15 156,25.072054 156,56 L156,62.1659169 C161.675427,63.1180731 166,68.0540074 166,74 L166,88 C166,94.0521096 161.51969,99.0577866 155.694861,99.8812385 C153.627775,119.687011 141.237365,136.422372 124,144.610951 Z\"\r\n      id=\"path-5\"\r\n          />\r\n  </defs>\r\n  <g\r\n    id=\"Avataaar\"\r\n    stroke=\"none\"\r\n    stroke-width=\"1\"\r\n    fill=\"none\"\r\n    fill-rule=\"evenodd\">\r\n    <g\r\n      transform=\"translate(-825.000000, -1100.000000)\"\r\n      id=\"Avataaar/Circle\">\r\n      <g transform=\"translate(825.000000, 1100.000000)\">\r\n        <g\r\n  id=\"Circle\"\r\n  stroke-width=\"1\"\r\n  fill-rule=\"evenodd\"\r\n  transform=\"translate(12.000000, 40.000000)\">\r\n  <mask id=\"mask-2\" fill=\"white\">\r\n    <use xlink:href=\"#path-1\" />\r\n  </mask>\r\n  <use\r\n    id=\"Circle-Background\"\r\n    fill=\"#E6E6E6\"\r\n    xlink:href=\"#path-1\"\r\n                  />\r\n  <g\r\n    id=\"Color/Palette/Blue-01\"\r\n    mask=\"url(#mask-2)\"\r\n    fill=\"#65C9FF\">\r\n    <rect id=\"🖍Color\" x=\"0\" y=\"0\" width=\"240\" height=\"240\" />\r\n  </g>\r\n</g>\r\n<mask id=\"mask-4\" fill=\"white\">\r\n  <use xlink:href=\"#path-3\" />\r\n</mask>\r\n        <g id=\"Mask\" />\r\n        <g\r\n          id=\"Avataaar\"\r\n          stroke-width=\"1\"\r\n          fill-rule=\"evenodd\"\r\n          mask=\"url(#mask-4)\">\r\n          <g id=\"Body\" transform=\"translate(32.000000, 36.000000)\">\r\n            <mask id=\"mask-6\" fill=\"white\">\r\n              <use xlink:href=\"#path-5\" />\r\n            </mask>\r\n            <use fill=\"#D0C6AC\" xlink:href=\"#path-5\" />\r\n            <g\r\n              id=\"Skin/👶🏽-03-Brown\"\r\n              mask=\"url(#mask-6)\"\r\n              fill=\"#FFDBB4\">\r\n              <g transform=\"translate(0.000000, 0.000000)\" id=\"Color\">\r\n                <rect x=\"0\" y=\"0\" width=\"264\" height=\"280\" />\r\n              </g>\r\n            </g>\r\n            <path\r\n              d=\"M156,79 L156,102 C156,132.927946 130.927946,158 100,158 C69.072054,158 44,132.927946 44,102 L44,79 L44,94 C44,124.927946 69.072054,150 100,150 C130.927946,150 156,124.927946 156,94 L156,79 Z\"\r\n              id=\"Neck-Shadow\"\r\n              fill-opacity=\"0.100000001\"\r\n              fill=\"#000000\"\r\n              mask=\"url(#mask-6)\"\r\n                  />\r\n          </g>\r\n          <g\r\n  id=\"Clothing/Shirt-Crew-Neck\"\r\n  transform=\"translate(0.000000, 170.000000)\">\r\n  <defs>\r\n    <path\r\n      d=\"M165.960472,29.2949161 C202.936473,32.3249982 232,63.2942856 232,101.051724 L232,110 L32,110 L32,101.051724 C32,62.9525631 61.591985,31.7649812 99.0454063,29.2195264 C99.0152598,29.5931145 99,29.9692272 99,30.3476251 C99,42.2107177 113.998461,51.8276544 132.5,51.8276544 C151.001539,51.8276544 166,42.2107177 166,30.3476251 C166,29.9946691 165.986723,29.6437014 165.960472,29.2949161 Z\"\r\n      id=\"clothing-shirt-crew-neck-path-1\"\r\n    />\r\n  </defs>\r\n  <mask id=\"clothing-shirt-crew-neck-mask-1\" fill=\"white\">\r\n    <use xlink:href=\"#clothing-shirt-crew-neck-path-1\" />\r\n  </mask>\r\n  <use\r\n    id=\"Clothes\"\r\n    fill=\"#E6E6E6\"\r\n    fill-rule=\"evenodd\"\r\n    xlink:href=\"#clothing-shirt-crew-neck-path-1\"\r\n  />\r\n  <g\r\n    id=\"Color/Palette/Gray-01\"\r\n    mask=\"url(#clothing-shirt-crew-neck-mask-1)\"\r\n    fill-rule=\"evenodd\"\r\n    fill=\"#A7FFC4\">\r\n    <rect id=\"🖍Color\" x=\"0\" y=\"0\" width=\"264\" height=\"110\" />\r\n  </g>\r\n  <g\r\n    id=\"Shadowy\"\r\n    opacity=\"0.599999964\"\r\n    stroke-width=\"1\"\r\n    fill-rule=\"evenodd\"\r\n    mask=\"url(#clothing-shirt-crew-neck-mask-1)\"\r\n    fill-opacity=\"0.16\"\r\n    fill=\"#000000\">\r\n    <g transform=\"translate(92.000000, 4.000000)\" id=\"Hola-👋🏼\">\r\n      <ellipse\r\n        cx=\"40.5\"\r\n        cy=\"27.8476251\"\r\n        rx=\"39.6351047\"\r\n        ry=\"26.9138272\"\r\n      />\r\n    </g>\r\n  </g>\r\n</g>\r\n          <g id=\"Face\" transform=\"translate(76.000000, 82.000000)\" fill=\"#000000\">\r\n            <g id=\"Mouth/Vomit\" transform=\"translate(2.000000, 52.000000)\">\r\n  <defs>\r\n    <path\r\n      d=\"M34.0082051,12.6020819 C35.1280248,23.0929366 38.2345159,31.9944054 53.9961505,31.9999974 C69.757785,32.0055894 72.9169073,23.0424631 73.9942614,12.5047938 C74.0809675,11.6567158 73.1738581,10.9999965 72.0369872,10.9999965 C65.3505138,10.9999965 62.6703194,12.4951994 53.9894323,12.4999966 C45.3085452,12.5047938 40.7567994,10.9999965 36.0924943,10.9999965 C34.9490269,10.9999965 33.8961688,11.5524868 34.0082051,12.6020819 Z\"\r\n      id=\"mouth-vomit-path-1\"\r\n    />\r\n    <path\r\n      d=\"M59.9170416,36 L60,36 C60,39.3137085 62.6862915,42 66,42 C69.3137085,42 72,39.3137085 72,36 L72,35 L72,31 C72,27.6862915 69.3137085,25 66,25 L66,25 L42,25 L42,25 C38.6862915,25 36,27.6862915 36,31 L36,31 L36,35 L36,38 C36,41.3137085 38.6862915,44 42,44 C45.3137085,44 48,41.3137085 48,38 L48,36 L48.0829584,36 C48.5590365,33.1622867 51.0270037,31 54,31 C56.9729963,31 59.4409635,33.1622867 59.9170416,36 Z\"\r\n      id=\"mouth-vomit-path-2\"\r\n    />\r\n    <filter\r\n      x=\"-1.4%\"\r\n      y=\"-2.6%\"\r\n      width=\"102.8%\"\r\n      height=\"105.3%\"\r\n      filterUnits=\"objectBoundingBox\"\r\n      id=\"mouth-vomit-filter-1\">\r\n      <feOffset\r\n        dx=\"0\"\r\n        dy=\"-1\"\r\n        in=\"SourceAlpha\"\r\n        result=\"shadowOffsetInner1\"\r\n      />\r\n      <feComposite\r\n        in=\"shadowOffsetInner1\"\r\n        in2=\"SourceAlpha\"\r\n        operator=\"arithmetic\"\r\n        k2=\"-1\"\r\n        k3=\"1\"\r\n        result=\"shadowInnerInner1\"\r\n      />\r\n      <feColorMatrix\r\n        values=\"0 0 0 0 0   0 0 0 0 0   0 0 0 0 0  0 0 0 0.1 0\"\r\n        type=\"matrix\"\r\n        in=\"shadowInnerInner1\"\r\n      />\r\n    </filter>\r\n  </defs>\r\n  <mask id=\"mouth-vomit-mask-1\" fill=\"white\">\r\n    <use\r\n      xlink:href=\"#mouth-vomit-path-1\"\r\n      transform=\"translate(54.000000, 21.499998) scale(1, -1) translate(-54.000000, -21.499998) \"\r\n    />\r\n  </mask>\r\n  <use\r\n    id=\"Mouth\"\r\n    fill-opacity=\"0.699999988\"\r\n    fill=\"#000000\"\r\n    fill-rule=\"evenodd\"\r\n    transform=\"translate(54.000000, 21.499998) scale(1, -1) translate(-54.000000, -21.499998) \"\r\n    xlink:href=\"#mouth-vomit-path-1\"\r\n  />\r\n  <rect\r\n    id=\"Teeth\"\r\n    fill=\"#FFFFFF\"\r\n    fill-rule=\"evenodd\"\r\n    mask=\"url(#mouth-vomit-mask-1)\"\r\n    x=\"39\"\r\n    y=\"0\"\r\n    width=\"31\"\r\n    height=\"16\"\r\n    rx=\"5\"\r\n  />\r\n  <g id=\"Vomit-Stuff\">\r\n    <use fill=\"#88C553\" fill-rule=\"evenodd\" xlink:href=\"#mouth-vomit-path-2\" />\r\n    <use\r\n      fill=\"black\"\r\n      fill-opacity=\"1\"\r\n      filter=\"url(#mouth-vomit-filter-1)\"\r\n      xlink:href=\"#mouth-vomit-path-2\"\r\n    />\r\n  </g>\r\n</g>\r\n            <g\r\n  id=\"Nose/Default\"\r\n  transform=\"translate(28.000000, 40.000000)\"\r\n  fill-opacity=\"0.16\">\r\n  <path\r\n    d=\"M16,8 C16,12.418278 21.372583,16 28,16 L28,16 C34.627417,16 40,12.418278 40,8\"\r\n    id=\"Nose\"\r\n  />\r\n</g>\r\n            <g\r\n  id=\"Eyes/Wink-😉\"\r\n  transform=\"translate(0.000000, 8.000000)\"\r\n  fill-opacity=\"0.599999964\">\r\n  <circle id=\"Eye\" cx=\"30\" cy=\"22\" r=\"6\" />\r\n  <path\r\n    d=\"M70.4123979,24.204889 C72.2589064,20.4060854 76.4166529,17.7575774 81.2498107,17.7575774 C86.065907,17.7575774 90.2113521,20.3874194 92.0675822,24.1647016 C92.618991,25.2867751 91.8343342,26.2050591 91.0428374,25.5246002 C88.5917368,23.4173607 85.1109468,22.1013658 81.2498107,22.1013658 C77.5094365,22.1013658 74.1259889,23.3363293 71.6897696,25.3292186 C70.7990233,26.0578718 69.8723316,25.3159619 70.4123979,24.204889 Z\"\r\n    id=\"Winky-Wink\"\r\n    transform=\"translate(81.252230, 21.757577) rotate(-4.000000) translate(-81.252230, -21.757577) \"\r\n  />\r\n</g>\r\n            <g\r\n  id=\"Eyebrow/Outline/Angry\"\r\n  fill-opacity=\"0.599999964\"\r\n  fill-rule=\"nonzero\">\r\n  <path\r\n    d=\"M15.6114904,15.1845247 C19.8515017,9.41618792 22.4892046,9.70087612 28.9238518,14.5564693 C29.1057771,14.6937504 29.2212592,14.7812575 29.5936891,15.063789 C34.4216439,18.7263562 36.7081807,20 40,20 C41.1045695,20 42,19.1045695 42,18 C42,16.8954305 41.1045695,16 40,16 C37.9337712,16 36.0986396,14.9777974 32.011227,11.8770179 C31.6358269,11.5922331 31.5189458,11.5036659 31.3332441,11.3635351 C27.5737397,8.52660822 25.3739873,7.28738405 22.6379899,6.99208688 C18.9538127,6.59445233 15.5799484,8.47367246 12.3885096,12.8154753 C11.7343147,13.7054768 11.9254737,14.9572954 12.8154753,15.6114904 C13.7054768,16.2656853 14.9572954,16.0745263 15.6114904,15.1845247 Z\"\r\n    id=\"Eyebrow\"\r\n  />\r\n  <path\r\n    d=\"M73.6114904,15.1845247 C77.8515017,9.41618792 80.4892046,9.70087612 86.9238518,14.5564693 C87.1057771,14.6937504 87.2212592,14.7812575 87.5936891,15.063789 C92.4216439,18.7263562 94.7081807,20 98,20 C99.1045695,20 100,19.1045695 100,18 C100,16.8954305 99.1045695,16 98,16 C95.9337712,16 94.0986396,14.9777974 90.011227,11.8770179 C89.6358269,11.5922331 89.5189458,11.5036659 89.3332441,11.3635351 C85.5737397,8.52660822 83.3739873,7.28738405 80.6379899,6.99208688 C76.9538127,6.59445233 73.5799484,8.47367246 70.3885096,12.8154753 C69.7343147,13.7054768 69.9254737,14.9572954 70.8154753,15.6114904 C71.7054768,16.2656853 72.9572954,16.0745263 73.6114904,15.1845247 Z\"\r\n    id=\"Eyebrow\"\r\n    transform=\"translate(84.999934, 13.470064) scale(-1, 1) translate(-84.999934, -13.470064) \"\r\n  />\r\n</g>\r\n          </g>\r\n          <g id=\"Top\" stroke-width=\"1\" fill-rule=\"evenodd\">\r\n        <defs>\r\n          <rect id=\"top-short-hair-waved-path-2\" x=\"0\" y=\"0\" width=\"264\" height=\"280\" />\r\n          <path\r\n            d=\"M183.679824,38.9488198 C189.086072,33.9985622 190.387393,23.9615454 187.317704,17.4493246 C183.549263,9.45385312 175.901319,8.45217737 168.572342,11.9686703 C161.664469,15.2835661 155.515175,16.3878671 147.950196,14.7817319 C140.691624,13.2406923 133.805566,10.5226172 126.303388,10.0762471 C113.978028,9.34292483 102.003367,13.914565 93.6031232,23.1292512 C92.0003538,24.8871655 90.7089493,26.8971594 89.4882796,28.9343872 C88.5115454,30.5644351 87.4105298,32.3133822 86.9950459,34.1921885 C86.7973853,35.0855929 87.165272,37.2898774 86.7203704,38.0218712 C86.2391099,38.8123183 84.4244668,39.5373375 83.6510124,40.1238625 C82.0842713,41.3125222 80.7267597,42.6539573 79.4713836,44.1710842 C76.8052796,47.3926541 75.3376994,50.7577001 74.1034777,54.7428152 C70.0005333,67.9877849 69.6528094,83.7412616 74.9569218,96.7467724 C75.6639385,98.4811062 77.8550622,102.098564 79.1431613,98.3847912 C79.3976741,97.6508047 78.8086588,95.1907873 78.8099809,94.4501584 C78.8146084,91.7300906 80.3160587,73.7213568 86.857084,63.6330196 C88.9862338,60.3491948 98.8298903,48.0522456 100.840541,47.9536058 C101.9058,49.6464245 112.720532,60.4624529 140.783385,59.1948919 C153.445253,58.6229725 163.18265,52.9341181 165.520833,50.4680909 C166.549375,56.0008881 178.51323,64.2839965 180.33625,67.6921976 C185.602529,77.5376948 186.770677,97.9957204 188.780988,97.9573368 C190.791299,97.9189532 192.234429,92.7197798 192.647929,91.7270713 C195.719601,84.351669 196.242509,75.0948338 195.914948,67.1684434 C195.487565,56.9663626 191.276535,45.9419513 183.679824,38.9488198 Z\"\r\n            id=\"top-short-hair-waved-path-1\"\r\n          />\r\n          <filter\r\n            x=\"-0.8%\"\r\n            y=\"-2.0%\"\r\n            width=\"101.5%\"\r\n            height=\"108.0%\"\r\n            filterUnits=\"objectBoundingBox\"\r\n            id=\"top-short-hair-waved-filter-1\">\r\n            <feOffset\r\n              dx=\"0\"\r\n              dy=\"2\"\r\n              in=\"SourceAlpha\"\r\n              result=\"shadowOffsetOuter1\"\r\n            />\r\n            <feColorMatrix\r\n              values=\"0 0 0 0 0   0 0 0 0 0   0 0 0 0 0  0 0 0 0.16 0\"\r\n              type=\"matrix\"\r\n              in=\"shadowOffsetOuter1\"\r\n              result=\"shadowMatrixOuter1\"\r\n            />\r\n            <feMerge>\r\n              <feMergeNode in=\"shadowMatrixOuter1\" />\r\n              <feMergeNode in=\"SourceGraphic\" />\r\n            </feMerge>\r\n          </filter>\r\n        </defs>\r\n        <mask id=\"top-short-hair-waved-mask-2\" fill=\"white\">\r\n          <use xlink:href=\"#top-short-hair-waved-path-2\" />\r\n        </mask>\r\n        <g id=\"Mask\" />\r\n        <g id=\"Top/Short-Hair/Short-Waved\" mask=\"url(#top-short-hair-waved-mask-2)\">\r\n          <g transform=\"translate(-1.000000, 0.000000)\">\r\n            <g\r\n  id=\"Facial-Hair/Beard-Light\"\r\n  transform=\"translate(49.000000, 72.000000)\">\r\n  <defs>\r\n    <path\r\n      d=\"M101.428403,98.1685688 C98.9148372,100.462621 96.23722,101.494309 92.8529444,100.772863 C92.2705777,100.648833 89.8963391,96.2345713 83.9998344,96.2345713 C78.1033297,96.2345713 75.7294253,100.648833 75.1467245,100.772863 C71.7624488,101.494309 69.0848316,100.462621 66.5712661,98.1685688 C61.8461772,93.855604 57.9166219,87.9081858 60.2778299,81.4191814 C61.5083844,78.0369425 63.5097479,74.3237342 67.1506257,73.2459109 C71.0384163,72.0955419 76.4968931,73.2439051 80.4147542,72.4582708 C81.6840664,72.2035248 83.0706538,71.7508657 83.9998344,71 C84.929015,71.7508657 86.3159365,72.2035248 87.5845805,72.4582708 C91.5027758,73.2439051 96.9612525,72.0955419 100.849043,73.2459109 C104.489921,74.3237342 106.491284,78.0369425 107.722173,81.4191814 C110.083381,87.9081858 106.153826,93.855604 101.428403,98.1685688 M140.081033,26 C136.670693,34.4002532 137.987774,44.8580348 137.356666,53.6758724 C136.844038,60.8431942 135.33712,71.5857526 128.972858,76.214531 C125.718361,78.5816138 119.79436,82.5598986 115.54187,81.4501943 C112.614539,80.6863848 112.302182,72.290096 108.455284,69.1469801 C104.09172,65.5823153 98.6429854,64.0160432 93.1491481,64.2578722 C90.7785381,64.3622683 85.9841367,64.3374908 83.9999331,66.1604584 C82.0157295,64.3374908 77.2216647,64.3622683 74.8510547,64.2578722 C69.3568808,64.0160432 63.9081467,65.5823153 59.5445817,69.1469801 C55.6976839,72.290096 55.3856641,80.6863848 52.4583326,81.4501943 C48.2058427,82.5598986 42.2818421,78.5816138 39.0270077,76.214531 C32.6624096,71.5857526 31.1561652,60.8431942 30.642864,53.6758724 C30.0120926,44.8580348 31.3291729,34.4002532 27.9188335,26 C26.2597768,26 27.3540339,42.1288693 27.3540339,42.1288693 L27.3540339,62.4851205 C27.3856735,77.7732046 36.935095,100.655445 58.1080116,109.393004 C63.2861266,111.52982 75.0153111,115 83.9999331,115 C92.9845551,115 104.71374,111.860188 109.891855,109.723371 C131.064771,100.985813 140.614193,77.7732046 140.646169,62.4851205 L140.646169,42.1288693 C140.646169,42.1288693 141.740089,26 140.081033,26\"\r\n      id=\"top-facial-hair-beard-light-path-1\"\r\n    />\r\n  </defs>\r\n  <mask id=\"top-facial-hair-beard-light-mask-1\" fill=\"white\">\r\n    <use xlink:href=\"#top-facial-hair-beard-light-path-1\" />\r\n  </mask>\r\n  <use\r\n    id=\"Lite-Beard\"\r\n    fill=\"#331B0C\"\r\n    fill-rule=\"evenodd\"\r\n    xlink:href=\"#top-facial-hair-beard-light-path-1\"\r\n  />\r\n  <g\r\n    id=\"Color/Hair/Brown\"\r\n    mask=\"url(#top-facial-hair-beard-light-mask-1)\"\r\n    fill=\"#4A312C\">\r\n    <g transform=\"translate(-32.000000, 0.000000)\" id=\"Color\">\r\n      <rect x=\"0\" y=\"0\" width=\"264\" height=\"244\" />\r\n    </g>\r\n  </g>\r\n</g>\r\n            <mask id=\"top-short-hair-waved-mask-1\" fill=\"white\">\r\n              <use xlink:href=\"#top-short-hair-waved-path-1\" />\r\n            </mask>\r\n            <use\r\n              id=\"Short-Hair\"\r\n              stroke=\"none\"\r\n              fill=\"#28354B\"\r\n              fill-rule=\"evenodd\"\r\n              xlink:href=\"#top-short-hair-waved-path-1\"\r\n            />\r\n            <g\r\n              id=\"Skin/👶🏽-03-Brown\"\r\n              mask=\"url(#top-short-hair-waved-mask-1)\"\r\n              fill=\"#A55728\">\r\n              <g transform=\"translate(0.000000, 0.000000) \" id=\"Color\">\r\n                <rect x=\"0\" y=\"0\" width=\"264\" height=\"280\" />\r\n              </g>\r\n            </g>\r\n            <g\r\n  id=\"Top/_Resources/Kurt\"\r\n  fill=\"none\"\r\n  transform=\"translate(62.000000, 85.000000)\"\r\n  stroke-width=\"1\">\r\n  <defs>\r\n    <filter\r\n      x=\"-0.8%\"\r\n      y=\"-2.0%\"\r\n      width=\"101.5%\"\r\n      height=\"108.0%\"\r\n      filterUnits=\"objectBoundingBox\"\r\n      id=\"top-resource-kurt-filter-1\">\r\n      <feOffset\r\n        dx=\"0\"\r\n        dy=\"2\"\r\n        in=\"SourceAlpha\"\r\n        result=\"shadowOffsetOuter1\"\r\n      />\r\n      <feColorMatrix\r\n        values=\"0 0 0 0 0   0 0 0 0 0   0 0 0 0 0  0 0 0 0.16 0\"\r\n        type=\"matrix\"\r\n        in=\"shadowOffsetOuter1\"\r\n        result=\"shadowMatrixOuter1\"\r\n      />\r\n      <feMerge>\r\n        <feMergeNode in=\"shadowMatrixOuter1\" />\r\n        <feMergeNode in=\"SourceGraphic\" />\r\n      </feMerge>\r\n    </filter>\r\n  </defs>\r\n  <g\r\n    id=\"Kurts\"\r\n    filter=\"url(#top-resource-kurt-filter-1)\"\r\n    transform=\"translate(5.000000, 2.000000)\">\r\n    <path\r\n      d=\"M66,11.1111111 C54.9625586,11.1111111 53.3705645,2.0266011 30.6705882,0.740740741 C7.98552275,-0.283199952 0.815225204,6.4494855 0.776470588,11.1111111 C0.813236892,15.4042795 -0.352293566,26.5612661 14.3647059,39.6296296 C29.1367705,55.1420807 44.2704162,49.8818301 49.6941176,44.8148148 C55.1352081,42.4731118 61.3403442,21.4596351 66,21.4814815 C70.6596558,21.5033279 76.8647919,42.4731118 82.3058824,44.8148148 C87.7295838,49.8818301 102.86323,55.1420807 117.635294,39.6296296 C132.352294,26.5612661 131.186763,15.4042795 131.223529,11.1111111 C131.184775,6.4494855 124.014477,-0.283199952 101.329412,0.740740741 C78.6294355,2.0266011 77.0374414,11.1111111 66,11.1111111 Z\"\r\n      id=\"It!\"\r\n      fill=\"#F4F4F4\"\r\n      fill-rule=\"nonzero\"\r\n    />\r\n    <path\r\n      d=\"M55.1294118,21.4814815 C55.5103632,13.8233491 42.2156493,5.64243259 27.9529412,5.92592593 C13.6973442,6.22450879 11.8417942,15.3786982 11.6470588,18.8888889 C11.2982286,27.0220633 20.014463,45.3037598 36.1058824,44.8148148 C52.1972736,44.305848 54.9092435,26.5344305 55.1294118,21.4814815 Z\"\r\n      id=\"Did\"\r\n      fill=\"#2F383B\"\r\n      fill-rule=\"nonzero\"\r\n    />\r\n    <path\r\n      d=\"M120.352941,21.4814815 C120.733893,13.8233491 107.439179,5.64243259 93.1764706,5.92592593 C78.9208736,6.22450879 77.0653236,15.3786982 76.8705882,18.8888889 C76.521758,27.0220633 85.2379924,45.3037598 101.329412,44.8148148 C117.420803,44.305848 120.132773,26.5344305 120.352941,21.4814815 Z\"\r\n      id=\"Courtney\"\r\n      fill=\"#2F383B\"\r\n      fill-rule=\"nonzero\"\r\n      transform=\"translate(98.611765, 25.370370) scale(-1, 1) translate(-98.611765, -25.370370) \"\r\n    />\r\n  </g>\r\n</g>\r\n          </g>\r\n        </g>\r\n      </g>\r\n        </g>\r\n      </g>\r\n    </g>\r\n  </g>\r\n</svg>";

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar");
            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());
            provider.InitializeRandomizer(seed);
            provider.Load(property, 1);
            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            // Act
            string actual = provider.GetRowValue(context, Array.Empty<string>());

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [Ignore]
        public void GetRowValueGeneratesOneMillionValues()
        {
            // Arrange
            var expected = new
            {
                RowCount = 1000000
            };

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar");

            List<string> generatedValues = new List<string>();
            AvatarSvgProvider provider = new AvatarSvgProvider();
            provider.InitializeRandomizer();
            provider.Load(property, 1000000);

            Dictionary<string, object>[] rows = new Dictionary<string, object>[1000000];

            for (int i = 0; i < 1000000; i++)
            {
                DataGeneratorContext context = new DataGeneratorContext(rows[i], i, expected.RowCount, property, property.Arguments);
                // Act
                generatedValues.Add(provider.GetRowValue(context, Array.Empty<string>()));
            }

            var actual = new
            {
                RowCount = generatedValues.Count(u => !string.IsNullOrEmpty(u)),
            };

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetValueIdReturnsExpectedValue()
        {
            // Arrange
            var expected = "svg goes here";

            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            var actual = provider.GetValueId(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSupportedArgumentsReturnsExpectedValues()
        {
            // Arrange
            AvatarSvgProvider provider = new AvatarSvgProvider(new AvatarSvgProviderConfiguration());

            // Act
            Dictionary<string, Type> arguments = provider.GetSupportedArguments();

            // Assert
            Assert.IsTrue(arguments.Count == 0);
        }

        [TestMethod]
        public void GetRowValueWithOneValidAccessoryReturnsExpectedValue()
        {
            // Arrange
            string expected = "Prescription-01";

            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidAccessories = new AvatarAccessory[]
                {
                    AvatarAccessory.Prescription01
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#929598";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidClotheTypes = new AvatarClotheType[]
                {
                    AvatarClotheType.Hoodie
                },
                ValidHatColors = new AvatarHatColor[]
                {
                     AvatarHatColor.Black
                },
                ValidClotheColors = new AvatarClotheColor[]
                {
                    AvatarClotheColor.Gray02
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Clothing/Hoodie\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidClotheTypes = new AvatarClotheType[]
                {
                    AvatarClotheType.Hoodie
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidClotheGraphicTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Clothing/Graphic/Bear\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidClotheTypes = new AvatarClotheType[]
                {
                    AvatarClotheType.GraphicShirt
                },
                ValidClotheGraphicTypes = new AvatarClotheGraphicType[]
                {
                    AvatarClotheGraphicType.Bear
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidEyebrowTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Eyebrow/Natural/Flat-Natural\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidEyebrowTypes = new AvatarEyebrowType[]
                {
                    AvatarEyebrowType.FlatNatural
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidEyeTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Eyes/Wink-Wacky-😜\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidEyeTypes = new AvatarEyeType[]
                {
                    AvatarEyeType.WinkWacky
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidFacialHairTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Facial-Hair/Beard-Majestic\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidFacialHairTypes = new AvatarFacialHairType[]
                {
                    AvatarFacialHairType.BeardMajestic
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidFacialHairColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#ECDCBF";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidHairColors = new AvatarHairColor[]
                {
                    AvatarHairColor.Black
                },
                ValidFacialHairTypes = new AvatarFacialHairType[]
                {
                    AvatarFacialHairType.BeardMajestic
                },
                ValidFacialHairColors = new AvatarFacialHairColor[]
                {
                    AvatarFacialHairColor.Platinum
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidHairColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#D6B370";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidFacialHairColors = new AvatarFacialHairColor[]
                {
                    AvatarFacialHairColor.Black
                },
                ValidTops = new AvatarTop[]
                {
                    AvatarTop.ShortHairShortCurly
                },
                ValidHairColors = new AvatarHairColor[]
                {
                    AvatarHairColor.BlondeGolden
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidHatColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#FFAFB9";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidClotheColors = new AvatarClotheColor[]
                {
                    AvatarClotheColor.Black
                },
                ValidTops = new AvatarTop[]
                {
                    AvatarTop.WinterHat1
                },
                ValidHatColors = new AvatarHatColor[]
                {
                    AvatarHatColor.PastelRed
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidMouthTypeReturnsExpectedValue()
        {
            // Arrange
            string expected = "id=\"Mouth/Twinkle\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidMouthTypes = new AvatarMouthType[]
                {
                    AvatarMouthType.Twinkle
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidSkinColorReturnsExpectedValue()
        {
            // Arrange
            string expected = "#D08B5B";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidSkinColors = new AvatarSkinColor[]
                {
                    AvatarSkinColor.Brown
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        [TestMethod]
        public void GetRowValueWithOneValidTopReturnsExpectedValue()
        {
            // Arrange
            string expected = "<rect id=\"top-long-hair-straight2-path-1\"";
            AvatarSvgProviderConfiguration configuration = new AvatarSvgProviderConfiguration()
            {
                ValidTops = new AvatarTop[]
                {
                    AvatarTop.LongHairStraight2
                }
            };

            ProviderResult providerResult = CreateProvider(configuration: configuration);
            AvatarSvgProvider provider = providerResult.Provider;

            // Act
            string actual = provider.GetRowValue(providerResult.Context, Array.Empty<string>());

            // Assert
            Assert.IsTrue(actual.Contains(expected));
        }

        private ProviderResult CreateProvider(
            int rowCount = 1,
            AvatarSvgProviderConfiguration? configuration = default,
            int? seed = default)
        {
            if (configuration == null)
            {
                configuration = new AvatarSvgProviderConfiguration();
            }

            AvatarSvgProvider provider = new AvatarSvgProvider(configuration);

            if (seed.HasValue)
            {
                provider.InitializeRandomizer(seed.Value);
            }
            else
            {
                provider.InitializeRandomizer();
            }

            DataGeneratorProperty<string> property = new DataGeneratorProperty<string>("Avatar")
                .FromProvider(provider);

            provider.Load(property, rowCount);

            DataGeneratorContext context = new DataGeneratorContext(new Dictionary<string, object>(), 0, 1, property, property.Arguments);

            return new ProviderResult()
            {
                Provider = provider,
                Property = property,
                Context = context
            };
        }

        private struct ProviderResult
        {
            public AvatarSvgProvider Provider;
            public DataGeneratorProperty<string> Property;
            public DataGeneratorContext Context;
        }
    }
}
