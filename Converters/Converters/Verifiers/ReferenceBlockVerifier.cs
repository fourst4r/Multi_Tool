using System.Collections.Generic;
using System.Linq;
using Converters.DataStructures;
using LevelModel.DTO;
using Converters.Converters.Components.ReferenceBlock;

namespace Converters.Converters.Verifiers
{
    internal class ReferenceLayerVerifier
    {


        private TmxBlockLayer _refLayer;
        private bool _refBlockFound;
        public bool IsValid { get; set; }

        private Messages _messages;


        public ReferenceLayerVerifier(List<TmxBlockLayer> layers, int refLayerID, Messages messages) {
            _messages = messages;
            IsValid = true;

            if (refLayerID != AddReferenceBlock.LAYER_ID)
                return;

            Verify(layers);
        }


        private void Verify(List<TmxBlockLayer> layers) {
            FindLayer(layers);
            VerifyLayer();
        }

        private void VerifyLayer() {

            if (_refLayer == null) {
                AddWarningMessage("Reference layer not found.");
                return;
            }

            VerifiyBlocks();

            if (_refBlockFound == false) 
                AddWarningMessage("Reference block not found.");
        }

        private void AddWarningMessage(string message, bool isValid = false) {
            IsValid = IsValid && isValid;
            _messages.Add(message);
        }

        private void FindLayer(List<TmxBlockLayer> layers) {
            var refLayer = layers.Where(l => l.IsRefLayer);

            if (refLayer == null || refLayer.Count() == 0) {
                AddWarningMessage("Reference layer not found.");
                return;
            }
            else if (refLayer.Count() > 1) {
                AddWarningMessage("There are too many reference layers in the TMX file.");
                return;
            }
            
            _refLayer = refLayer.First();
        }

        private void VerifiyBlocks() {
            int height = _refLayer.BlockArray.GetLength(0);
            int width = _refLayer.BlockArray.GetLength(1);

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    VerifyBlock(_refLayer.BlockArray[y, x]);
                }
            }
        }

        private void VerifyBlock(int id) { 
            if (id == TmxBlocks.NO_BLOCK) {
                return;
            }
            else if (id != AddReferenceBlock.REFERENCE_BLOCK_ID) {
                AddWarningMessage("The reference layer should only contain the reference block.", true);
            }
            else {
                RefBlockFound();
            }
        }

        private void RefBlockFound() {
            if (_refBlockFound) 
                AddWarningMessage("The reference layer contains too many blocks.");

            _refBlockFound = true;
        }


    }
}
