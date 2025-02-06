// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Smoke"
{
	Properties
	{
		_TextureSample2("Texture Sample 0", 2D) = "white" {}
		_Distortion("Distortion", Range( 0 , 1)) = 0.7217394

	}

	SubShader
	{
		

		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		

		
		Pass
		{
			Name "Unlit"

			CGPROGRAM

			#define ASE_VERSION 19801


			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _TextureSample2;
			uniform float _Distortion;


			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float2 temp_cast_0 = (1.0).xx;
				float2 texCoord94 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner99 = ( 1.0 * _Time.y * temp_cast_0 + texCoord94);
				
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = ( tex2Dlod( _TextureSample2, float4( panner99, 0, 0.0) ).r * v.ase_normal * _Distortion );
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}

			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				

				finalColor = fixed4(1,1,1,1);
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.TextureCoordinatesNode;94;7264,1088;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;95;7392,1296;Inherit;False;Constant;_Float5;Float 0;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;99;7552,1088;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;49;1540.803,2439.138;Inherit;False;1831.834;771.4982;Hover sur les nodes pour avoir la tooltip explicative.;12;74;71;66;64;63;62;61;60;59;58;57;56;Depth Intersection setup.;0.3537736,1,0.6119694,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;50;3794.803,1237.138;Inherit;False;915;401;;6;96;92;89;85;70;65;Attribute color to Fresnel;1,0.7971698,0.7971698,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;51;2052.803,1735.138;Inherit;False;1253.283;495.6732;Paramètres en property pour modifier le rendu de notre Fresnel à notre guise. Le résultat est clampé pour empêcher celui-ci d'atteindre un HDR trop élever et de générer des artefact.;7;80;77;76;72;69;68;67;Fresnel Setup;0.7783019,0.9759024,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;52;3458.803,2581.138;Inherit;False;1508.56;353.0114;Comment;8;91;87;84;83;79;78;75;73;Attribute the same color as the Fresnel;1,0.7960784,0.7960784,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;53;3824,1872;Inherit;False;982.5397;351.4486;On récupère l'inverse de notre Fresnel, puis on multiplie le tout par une couleur afin de remplir notre forme.;4;97;90;88;86;Attribute Color to the inside of the Mesh;1,0.9009434,0.6556604,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;54;5428.803,1383.138;Inherit;False;541;304;;2;98;93;With Transparent Inside.;1,1,1,0.6666667;0;0
Node;AmplifyShaderEditor.CommentaryNode;55;5968,1760;Inherit;False;541;304;;2;101;100;With Opaque & Colored inside.;0,0,0,1;0;0
Node;AmplifyShaderEditor.SamplerNode;102;7808,1072;Inherit;True;Property;_TextureSample2;Texture Sample 0;12;0;Create;True;0;0;0;False;0;False;-1;04dc6b0fd0c2b004b834e1dc0ea7a8b3;04dc6b0fd0c2b004b834e1dc0ea7a8b3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.NormalVertexDataNode;103;7632,1344;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;104;7904,1376;Inherit;False;Property;_Distortion;Distortion;13;0;Create;True;0;0;0;False;0;False;0.7217394;0.7217394;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;1572.803,2823.138;Inherit;False;Property;_Intersection_Width;Intersection_Width;9;0;Create;True;0;0;0;False;0;False;0;1.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;57;1604.803,2615.138;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;58;1860.803,2823.138;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenDepthNode;59;1604.803,2535.138;Inherit;False;0;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;2308.803,2823.138;Inherit;False;Constant;_Intersection_Max;Intersection_Max;9;0;Create;True;0;0;0;False;0;False;0;0.16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;2308.803,2903.138;Inherit;False;Property;_Intersection_Smoothness;Intersection_Smoothness;11;0;Create;True;0;0;0;False;0;False;0;24.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;2132.803,2663.138;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;63;2564.803,2855.138;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;64;2276.803,2567.138;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;66;2724.803,2567.138;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;65;3844.803,1287.138;Inherit;False;Property;_Fresnel_Color;Fresnel_Color;0;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;67;2100.803,1895.138;Inherit;False;Property;_F_Scale;F_Scale;3;0;Create;True;0;0;0;False;0;False;1;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;2100.803,1815.138;Inherit;False;Property;_F_Bias;F_Bias;4;0;Create;True;0;0;0;False;0;False;0;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;2100.803,1975.138;Inherit;False;Property;_F_Power;F_Power;2;0;Create;True;0;0;0;False;0;False;5;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;70;4100.803,1287.138;Inherit;False;Fresnel_Color;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;71;2900.803,2567.138;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;72;2356.803,1815.138;Inherit;True;Standard;WorldNormal;ViewDir;False;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;73;3508.803,2727.138;Inherit;False;70;Fresnel_Color;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;74;3140.803,2583.138;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;3748.803,2631.138;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;76;2740.803,1991.138;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;77;2740.803,1799.138;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;78;4004.803,2631.138;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;79;4040.803,2816.138;Inherit;False;Property;_Intersection_HDR;Intersection_HDR;10;0;Create;True;0;0;0;False;0;False;0;0.07;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;80;3028.803,1991.138;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;3492.803,1799.138;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;3492.803,1991.138;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;83;4004.803,2727.138;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;4292.803,2631.138;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;4100.803,1415.138;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;86;3824,2000;Inherit;False;Property;_Base_Color;Base_Color;5;0;Create;True;0;0;0;False;0;False;0.490566,0.490566,0.490566,1;0.1396227,0.1325098,0.1325098,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.DynamicAppendNode;87;4548.803,2631.138;Inherit;False;COLOR;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;89;4244.803,1415.138;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;4144,1904;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;90;4368,1920;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;91;4708.803,2631.138;Inherit;False;Colored_Intersection;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;92;4388.803,1527.138;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;39;5840,-176;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;96;4548.803,1415.138;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;97;4576,1920;Inherit;False;Inside_Colored;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;93;5312,1504;Inherit;False;91;Colored_Intersection;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;20;6688,-160;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;6688,-336;Inherit;False;Property;_Purity;Purity;8;0;Create;True;0;0;0;False;0;False;1;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;98;5732.803,1431.138;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;100;6016,1840;Inherit;False;97;Inside_Colored;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;152;5920,656;Inherit;False;Property;_Multiply_Lighten;Multiply_Lighten;14;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;7008,-576;Inherit;False;Property;_Vanish_Time;Vanish_Time;1;0;Create;True;0;0;0;False;0;False;1;1.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;11;7120,-160;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;101;6272,1808;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;6256,464;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;14;7376,-512;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;7408,-208;Inherit;False;Property;_Smoothstep_Float;Smoothstep_Float;7;0;Create;True;0;0;0;False;0;False;0.2;1.26;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;106;7120,880;Inherit;False;Property;_Transparent_Inside;Transparent_Inside;6;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;154;6608,368;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;149;7104,400;Inherit;False;Constant;_Float0;Float 0;18;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;21;7664,-416;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;148;7440,464;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;16;7824,-256;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;157;7712,176;Inherit;False;False;False;False;True;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;8048,-48;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;105;8192,272;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;156;8224,16;Inherit;False;COLOR;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;163;8400,64;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;100;5;Smoke;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;False;True;0;1;False;;0;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;1;False;;True;3;False;;True;True;0;False;;0;False;;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;1;True;False;;False;0
WireConnection;99;0;94;0
WireConnection;99;2;95;0
WireConnection;102;1;99;0
WireConnection;58;0;56;0
WireConnection;62;0;57;4
WireConnection;62;1;58;0
WireConnection;63;0;60;0
WireConnection;63;1;61;0
WireConnection;64;0;59;0
WireConnection;64;1;62;0
WireConnection;66;0;64;0
WireConnection;66;1;63;0
WireConnection;66;2;60;0
WireConnection;70;0;65;0
WireConnection;71;0;66;0
WireConnection;72;1;68;0
WireConnection;72;2;67;0
WireConnection;72;3;69;0
WireConnection;74;0;71;0
WireConnection;75;0;74;0
WireConnection;75;1;73;0
WireConnection;76;0;72;0
WireConnection;77;0;72;0
WireConnection;78;0;75;0
WireConnection;80;0;76;0
WireConnection;81;0;77;0
WireConnection;81;1;71;0
WireConnection;82;0;80;0
WireConnection;82;1;71;0
WireConnection;83;0;75;0
WireConnection;84;0;78;0
WireConnection;84;1;79;0
WireConnection;85;0;65;0
WireConnection;85;1;81;0
WireConnection;87;0;84;0
WireConnection;87;3;83;0
WireConnection;89;0;85;0
WireConnection;88;0;82;0
WireConnection;88;1;86;0
WireConnection;90;0;88;0
WireConnection;91;0;87;0
WireConnection;92;0;89;3
WireConnection;96;0;89;0
WireConnection;96;1;89;1
WireConnection;96;2;89;2
WireConnection;96;3;92;0
WireConnection;97;0;90;0
WireConnection;20;0;39;3
WireConnection;98;0;96;0
WireConnection;98;1;93;0
WireConnection;11;0;48;0
WireConnection;11;2;20;0
WireConnection;101;0;98;0
WireConnection;101;1;100;0
WireConnection;153;0;39;3
WireConnection;153;1;152;0
WireConnection;14;0;15;0
WireConnection;14;1;11;0
WireConnection;106;1;101;0
WireConnection;106;0;98;0
WireConnection;154;0;153;0
WireConnection;21;0;14;0
WireConnection;21;2;22;0
WireConnection;148;0;106;0
WireConnection;148;1;149;0
WireConnection;148;2;154;0
WireConnection;16;0;21;0
WireConnection;157;0;148;0
WireConnection;158;0;16;0
WireConnection;158;1;157;0
WireConnection;105;0;102;1
WireConnection;105;1;103;0
WireConnection;105;2;104;0
WireConnection;156;0;148;0
WireConnection;156;3;158;0
WireConnection;163;1;105;0
ASEEND*/
//CHKSM=DF08C9F290CAC90A12EA45351DC503EA2A95C6D1