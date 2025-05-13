Shader "Custom/WorldGrid2"
{
    Properties
    {
        _BgColor       ("背景颜色", Color)   = (1,1,1,1)
        _LineColor     ("网格线颜色", Color)   = (0,0,0,1)
        _GridWidth     ("网格单元宽度", Float) = 1.0
        _GridHeight    ("网格单元高度", Float) = 1.0
        _LineThickness ("网格线粗细", Float)   = 0.05
        
        _WorldSize     ("世界长宽（脚本设置）",Vector) =(25,25,0,0)
        _ColorMaskTex   ("染色贴图", 2D)        = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        CGPROGRAM
        // 使用 Lambert 光照模型，支持光照
        #pragma surface surf Lambert

        fixed4 _BgColor;
        fixed4 _LineColor;
        float _GridWidth;
        float _GridHeight;
        float _LineThickness;

        fixed4 _WorldSize;
        sampler2D _ColorMaskTex;

        struct Input
        {
            float3 worldPos; // 使用世界坐标来计算网格位置
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // 计算当前像素在 XZ 平面上的网格坐标
            float2 uv = (IN.worldPos.xz +float2(0.5*_GridWidth,0.5*_GridHeight)) / float2(_GridWidth, _GridHeight);
            // 取小数部分，即单元格内部坐标（范围 0 ~ 1）
            float2 f = frac(uv);

            // 计算在每个方向上离最近边界的距离
            float dx = min(f.x, 1.0 - f.x);
            float dy = min(f.y, 1.0 - f.y);

            // 根据屏幕空间微分实现抗锯齿平滑效果
            float antialiasX = fwidth(f.x);
            float antialiasY = fwidth(f.y);

            // 如果距离网格线小于指定粗细，则输出网格线颜色；使用 smoothstep 平滑过渡
            float lineX = 1.0 - smoothstep(_LineThickness, _LineThickness + antialiasX, dx);
            float lineY = 1.0 - smoothstep(_LineThickness, _LineThickness + antialiasY, dy);
            
            // 两个方向上只要有一个靠近边界就显示网格线（取最大值）
            float l = max(lineX, lineY);
            l = saturate(l); // 限制范围在 0～1

              // 采样染色纹理，获取当前网格是否被染色
            float2 maskUV = (IN.worldPos.xz-_WorldSize.xy) / (_WorldSize.zw-_WorldSize.xy);  // 归一化坐标（假设地图大小100x100）
            fixed4 maskColor = tex2D(_ColorMaskTex, maskUV);

            fixed4 bgColor=_BgColor;
            if(maskColor.a>0.5)
                bgColor=maskColor;

            // 根据网格线强度在背景色和网格线颜色之间做插值
            fixed4 col = lerp(bgColor, _LineColor, l);

           
            
            o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
