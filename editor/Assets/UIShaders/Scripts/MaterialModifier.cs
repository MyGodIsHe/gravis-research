using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CeilingLight.UIShaders {

    /// <summary>
    /// Material modifier. Class to customize a single instance
    /// of a material without having to create different material assets
    /// for every version of the same material you want.
    /// The instance material is created at runtime in the Awake method
    /// and you can define which shader parameter change and when.
    /// and it's automatically removed when the excution stops.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public abstract class MaterialModifier : MonoBehaviour {

        Material matInstance;

        void Awake () {
            var image = GetComponent<Image>();

            /* Not super optimized but seems the only solution for 
               modifing singular Image's material since we can't access
               Renderer component and PropertyBlock.
               Works also for animating the parameters in the Update */
            matInstance = Instantiate(image.material);
            setParams(createInitialParams());
            image.material = matInstance;
            enabled = false;
        }

        /// <summary>
        /// Sets all the material parameters of the material
        /// </summary>
        /// <param name="_params">List of parameters.</param>
        protected void setParams(List<MaterialParam> _params) {
            for (int index = 0; index < _params.Count; index++) {
                var param = _params.ElementAt(index);
                setParam(param);
            }
        }

        /// <summary>
        /// Sets a single parameter using a different setting function
        /// based on its type.
        /// </summary>
        /// <param name="_param">Parameter to set.</param>
        protected void setParam(MaterialParam _param) {
            if(matInstance.HasProperty(_param.name)) {
                switch (_param.type) {
                case MaterialParam.ParamType.COLOR:
                    matInstance.SetColor(_param.name, (Color) _param.value);
                    break;
                case MaterialParam.ParamType.FLOAT:
                    matInstance.SetFloat(_param.name, Convert.ToSingle(_param.value));
                    break;
                case MaterialParam.ParamType.TEXTURE:
                    matInstance.SetTexture(_param.name, _param.value as Texture);
                    break;
                }
            }
            else {
                Debug.LogWarning(String.Format("The {0} material doesn't have a property named {1}", matInstance.name, _param.name));
            }
        }

        /// <summary>
        /// Creates the parameters.
        /// The implementation has to create all the parameters you want to change
        /// in the material.
        /// Since Unity at the moment does not allow to know all the material's properties 
        /// at runtime (but only in the editor with ShaderUtil), you have to be careful of what you
        /// are creating and setting.
        /// </summary>
        /// <returns>The parameters as a list.</returns>
        protected abstract List<MaterialParam> createInitialParams();


        /// <summary>
        /// Internal class for storing the parameter data.
        /// </summary>
        public class MaterialParam {
            public enum ParamType {
                COLOR,
                FLOAT,
                TEXTURE
            }

            public string name         { get; private set; }
            public ParamType type    { get; private set; }
            public object value     { get; private set; }


            public MaterialParam(string _name, ParamType _type, object _value) {
                name = _name;
                type = _type;
                value = _value;
            }
        }
    }
}
