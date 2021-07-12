using System;
using System.Collections.Generic;
using System.Text;
using SiGlaz.ML.NNFramework;
using System.IO;

using SiGlaz.FeatureProcessing;

namespace SiGlaz.Classifier.NN
{
    using IntCollection = List<int>;

    public class NNClassifier : IClassifier
    {
        private NeuralNetworkStructure _nnStructure = null;
        private NeuralNetworkController _nnController = null;
        public void InitNNController(string nnsfile)
        {
            NeuralNetworkStructure nns = null;

            using (FileStream fs = new FileStream(nnsfile, FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fs))
                nns = NeuralNetworkStructure.Deserialize(reader);

            _nnStructure = nns;
            _nnController = new NeuralNetworkController();
            _nnController.CreateNetwork(nns);
        }

        public void InitNNController(BinaryReader reader)
        {
            NeuralNetworkStructure nns = null;

            nns = NeuralNetworkStructure.Deserialize(reader);

            _nnStructure = nns;
            _nnController = new NeuralNetworkController();
            _nnController.CreateNetwork(nns);
        }

        public int Classify(IFeatureVector fv)
        {
            return Classify(_nnController, fv);
        }

        public int[] Classify(FeatureVectorCollection fvs)
        {
            List<int> classIds = new List<int>(fvs.Count);

            int n = fvs.Count;

            for (int i = 0; i < n; i++)
            {
                classIds.Add(this.Classify(_nnController, fvs[i]));
            }

            return classIds.ToArray();
        }

        public int[] ClassifyParallely(FeatureVectorCollection fvs)
        {
            int n = fvs.Count;
            int nThreads = Parallel.ThreadsCount;
            if (nThreads > n)
                nThreads = 1;

            int nFVsPerThread = n / nThreads;

            int[] classIds = new int[n];
            int[] idxStartThreads = new int[nThreads];
            int[] idxEndThreads = new int[nThreads];
                       
            NeuralNetworkController[] nnControllers = new NeuralNetworkController[nThreads];
            for (int i = 0; i < nThreads; i++)
            {
                nnControllers[i] = new NeuralNetworkController();
                nnControllers[i].CreateNetwork(_nnStructure);

                idxStartThreads[i] = i * nFVsPerThread;
                idxEndThreads[i] = idxStartThreads[i] + nFVsPerThread - 1;
            }

            Parallel.For(0, nThreads, delegate(int iThread)
            {
                Classify(
                    nnControllers[iThread], fvs, 
                    idxStartThreads[iThread], idxEndThreads[iThread], 
                    classIds);
            });

            int idxStartRemains = idxEndThreads[nThreads-1] + 1;
            int idxEndRemains = n-1;
            if (idxEndRemains >= idxStartRemains)
            {
                Parallel.For(idxStartRemains, idxEndRemains, delegate(int iRemain)
                {
                    Classify(
                        nnControllers[iRemain - idxStartRemains], fvs, iRemain, iRemain, classIds);
                });
            }

            return classIds;
        }

        private int Classify(
            NeuralNetworkController nnController, IFeatureVector fv)
        {
            if (fv == null || fv.Features == null)
                return -1;

            double confidence = 0;

            return nnController.Recognize(fv.Features, ref confidence);
        }

        private void Classify(
            NeuralNetworkController nnController, 
            FeatureVectorCollection fvs, 
            int start, int end, int[] classIds)
        {
            for (int i = start; i <= end; i++)
            {
                classIds[i] = this.Classify(nnController, fvs[i]);
            }
        }
    }
}
