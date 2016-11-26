using UnityEngine;
using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
namespace Leap.Unity.DetectionExamples
{

    public class PinchDraw : MonoBehaviour
    {

        [Tooltip("Each pinch detector can draw one line at a time.")]
        [SerializeField]
        private PinchDetector[] _pinchDetectors;

        [SerializeField]
        private Material _material;

        [SerializeField]
        private Color _drawColor = Color.white;

        [SerializeField]
        private float _smoothingDelay = 0.01f;

        [SerializeField]
        private float _drawRadius = 0.002f;

        [SerializeField]
        private int _drawResolution = 8;

        [SerializeField]
        private float _minSegmentLength = 0.005f;

        private DrawState[] _drawStates;

        //user-defined
        //time
        private float currTime;
        private float endTime = -1;
        //position
        private Vector3 startPosition;
        private Vector3 endPosition;
        //flag
        private bool flag_drawing = false;//drawing an image
        private bool flag_recognize = false;
        //count
        private int num_drawline = 0;
        //Leap
        private Hand detectedHand;
        //Gesture Status
        private int Lstatus = 0;//1-down, 2-right after down
        private int Vstatus = 0;//1-down, 2-up after down

        //Line
        private List<GameObject> lineObj = new List<GameObject>();

        //Speech Recognition
        private KeywordRecognizer m_Recognizer;
        //Dictionary<string, System.Action> keywords = new Dictionary<string, Action>();
        private string[] keywords= {
            "listen",
            "license",
            "like",
            "least",
            "to",
            "your",
            "heart",
            "excuse",
            "go",
            "tell",
            "me",
            "try",
            "li",
            "son",
            "two",
            "too",
            "yo",
            "hard",
            "art",
            "ex",
            "cue",
            "valiant",
            "gate",
            "obligate",
            "ter",
            "ant"
        };
  

        public Color DrawColor
        {
            get
            {
                return _drawColor;
            }
            set
            {
                _drawColor = value;
            }
        }

        public float DrawRadius
        {
            get
            {
                return _drawRadius;
            }
            set
            {
                _drawRadius = value;
            }
        }

        void OnValidate()
        {
            _drawRadius = Mathf.Max(0, _drawRadius);
            _drawResolution = Mathf.Clamp(_drawResolution, 3, 24);
            _minSegmentLength = Mathf.Max(0, _minSegmentLength);
        }

        void Awake()
        {
            if (_pinchDetectors.Length == 0)
            {
                Debug.LogWarning("No pinch detectors were specified!  PinchDraw can not draw any lines without PinchDetectors.");
            }
        }

        

        //private void speechListen()
        //{
        //    Debug.Log("Get listen speaking.\n");
        //}

        void Start()
        {
            _drawStates = new DrawState[_pinchDetectors.Length];
            for (int i = 0; i < _pinchDetectors.Length; i++)
            {
                _drawStates[i] = new DrawState(this);
            }
            //speech recognition
            //keywords.Add("Go", () =>
            //{
            //    speechListen();
            //});
            m_Recognizer = new KeywordRecognizer(keywords);
            m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
            m_Recognizer.Start();
        }

        private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            //System.Action keywordAction;

            //if (keywords.TryGetValue(args.text, out keywordAction))
            //{
            //    keywordAction.Invoke();
            //}
            Debug.Log("Keyword: " + args.text + "; Confidence: " + args.confidence + "; Start Time: " + args.phraseStartTime + "; Duration: " + args.phraseDuration);

        }

        void Update()
        {
            //if (m_Recognizer.IsRunning)
            //{
            //    Debug.Log("Get keywords.\n");
            //}
            
            //m_Recognizer.


            currTime = Time.time;

            //test every detector
            for (int i = 0; i < _pinchDetectors.Length; i++)
            {

                var detector = _pinchDetectors[i];
                var drawState = _drawStates[i];

                try
                {
                    detectedHand = detector.HandModel.GetLeapHand();
                    if (detectedHand.IsRight)
                    {
                        if (detector.DidStartHold)
                        {
                            Debug.Log("Begin pinch...\n");
                            lineObj.Add(drawState.BeginNewLine());
                            //user

                            if (!flag_drawing)
                            {
                                startPosition = detector.Position;
                                flag_drawing = true;
                                flag_recognize = false;
                                
                            }

                        }

                        if (detector.DidRelease)
                        {
                            //Debug.Log("The number of draw line before is :" + num_drawline + ".\n");
                            Debug.Log("Now released.\n");
                            drawState.FinishLine();
                            endTime = Time.time;
                            endPosition = detector.Position;
                            num_drawline += 1;
                            //Debug.Log("The number of draw line after is :" + num_drawline + ".\n");


                        }

                        if (detector.IsHolding)
                        {
                            //Debug.Log("Now holding ..");
                            drawState.UpdateLine(detector.Position);

                            if (!flag_drawing)
                            {
                                startPosition = detector.Position;
                                flag_drawing = true;
                                flag_recognize = false;

                            }

                            if (//down
                                Mathf.Abs(detectedHand.PalmVelocity.y) > Mathf.Abs(detectedHand.PalmVelocity.x)
                                //&&
                                //Mathf.Abs(detectedHand.PalmVelocity.y) > Mathf.Abs(detectedHand.PalmVelocity.z)
                                &&
                                detectedHand.PalmVelocity.y < 0
                                )
                            {
                                if (Lstatus == 0)
                                {
                                    Lstatus = 1;
                                }

                                if (Vstatus == 0)
                                {
                                    Vstatus = 1;
                                }
                            }
                            else if (//up
                               Mathf.Abs(detectedHand.PalmVelocity.y) > Mathf.Abs(detectedHand.PalmVelocity.x)
                               //&&
                               //Mathf.Abs(detectedHand.PalmVelocity.y) > Mathf.Abs(detectedHand.PalmVelocity.z)
                               &&
                               detectedHand.PalmVelocity.y > 0
                               )
                            {


                                if (Vstatus == 1)
                                {
                                    Vstatus = 2;
                                }
                                else
                                {
                                    //Vstatus = 0;
                                }

                                //Lstatus = 0;
                            }
                            else if (//left
                                Mathf.Abs(detectedHand.PalmVelocity.x) > Mathf.Abs(detectedHand.PalmVelocity.y)
                               //&&
                               //Mathf.Abs(detectedHand.PalmVelocity.x) > Mathf.Abs(detectedHand.PalmVelocity.z)
                               &&
                               detectedHand.PalmVelocity.x < 0
                               )
                            {
                                //Lstatus = 0;
                                //Vstatus = 0;

                            }
                            else if (//right
                                Mathf.Abs(detectedHand.PalmVelocity.x) > Mathf.Abs(detectedHand.PalmVelocity.y)
                               //&&
                               //Mathf.Abs(detectedHand.PalmVelocity.x) > Mathf.Abs(detectedHand.PalmVelocity.z)
                               &&
                               detectedHand.PalmVelocity.x > 0
                               )
                            {
                                if (Lstatus == 1)
                                {
                                    Lstatus = 2;
                                }
                                //Vstatus = 0;

                            }



                        }

                        //check if draw finished
                        if (currTime - endTime > 2 && endTime != -1 && !flag_recognize && !detector.IsHolding && flag_drawing)
                        {

                            endTime = -1;
                            flag_recognize = true;
                            flag_drawing = false;
                            //drawState.deleteLine();

                            float distBetweenStartAndEnd = Vector3.Distance(new Vector3(startPosition.x, startPosition.y), new Vector3(endPosition.x, endPosition.y));
                            //Debug.Log("The start position is x : " + startPosition.x + ", y : " + startPosition.y + ", z: " + startPosition.z + ".\n");
                            //Debug.Log("The end position is x : " + endPosition.x + ", y : " + endPosition.y + ", z: " + endPosition.z + ".\n");
                            //Debug.Log("The distance is : " + distBetweenStartAndEnd + "\n");

                            if (num_drawline >= 3)
                            {
                                Debug.Log("You have drawn E");
                            }
                            else if (distBetweenStartAndEnd < 0.068)
                            {
                                Debug.Log("You have drawn O!");
                            }
                            else
                            {
                                if (Lstatus == 2 && Vstatus == 2)
                                {
                                    float yDistBetweenStartAndEnd = Mathf.Abs(startPosition.y - endPosition.y);
                                    if (yDistBetweenStartAndEnd > 0.1)
                                    {
                                        Debug.Log("You have drawn L!");

                                    }
                                    else
                                    {
                                        Debug.Log("You have drawn V!");

                                    }
                                }
                                else if (Lstatus == 2)
                                {
                                    Debug.Log("You have drawn L!");
                                }
                                else if (Vstatus == 2)
                                {
                                    Debug.Log("You have drawn V!");

                                }

                            }

                            //Debug.Log("The number of draw line is :" + num_drawline + ".\n");
                            num_drawline = 0;
                            Lstatus = 0;
                            Vstatus = 0;
                            foreach (GameObject obj in lineObj)
                            {
                                Destroy(obj);
                            }
                            lineObj.Clear();
                        }
                    }
                }
                catch (System.Exception)
                {

                    //throw;
                }







            }


        }

        private class DrawState
        {
            private List<Vector3> _vertices = new List<Vector3>();
            private List<int> _tris = new List<int>();
            private List<Vector2> _uvs = new List<Vector2>();
            private List<Color> _colors = new List<Color>();

            private PinchDraw _parent;

            private int _rings = 0;

            private Vector3 _prevRing0 = Vector3.zero;
            private Vector3 _prevRing1 = Vector3.zero;

            private Vector3 _prevNormal0 = Vector3.zero;

            private Mesh _mesh;
            private SmoothedVector3 _smoothedPosition;

            public DrawState(PinchDraw parent)
            {
                _parent = parent;

                _smoothedPosition = new SmoothedVector3();
                _smoothedPosition.delay = parent._smoothingDelay;
                _smoothedPosition.reset = true;
            }

            public GameObject BeginNewLine()
            {
                _rings = 0;
                _vertices.Clear();
                _tris.Clear();
                _uvs.Clear();
                _colors.Clear();

                _smoothedPosition.reset = true;

                _mesh = new Mesh();
                _mesh.name = "Line Mesh";
                _mesh.MarkDynamic();

                GameObject lineObj = new GameObject("Line Object");
                lineObj.transform.position = Vector3.zero;
                lineObj.transform.rotation = Quaternion.identity;
                lineObj.transform.localScale = Vector3.one;
                lineObj.AddComponent<MeshFilter>().mesh = _mesh;
                lineObj.AddComponent<MeshRenderer>().sharedMaterial = _parent._material;

                return lineObj;
            }

            public void UpdateLine(Vector3 position)
            {
                _smoothedPosition.Update(position, Time.deltaTime);

                bool shouldAdd = false;

                shouldAdd |= _vertices.Count == 0;
                shouldAdd |= Vector3.Distance(_prevRing0, _smoothedPosition.value) >= _parent._minSegmentLength;

                if (shouldAdd)
                {
                    addRing(_smoothedPosition.value);
                    updateMesh();
                }
            }

            public void FinishLine()
            {
                _mesh.Optimize();
                _mesh.UploadMeshData(true);
            }

            private void updateMesh()
            {
                _mesh.SetVertices(_vertices);
                _mesh.SetColors(_colors);
                _mesh.SetUVs(0, _uvs);
                _mesh.SetIndices(_tris.ToArray(), MeshTopology.Triangles, 0);
                _mesh.RecalculateBounds();
                _mesh.RecalculateNormals();
            }

            private void addRing(Vector3 ringPosition)
            {
                _rings++;

                if (_rings == 1)
                {
                    addVertexRing();
                    addVertexRing();
                    addTriSegment();
                }

                addVertexRing();
                addTriSegment();

                Vector3 ringNormal = Vector3.zero;
                if (_rings == 2)
                {
                    Vector3 direction = ringPosition - _prevRing0;
                    float angleToUp = Vector3.Angle(direction, Vector3.up);

                    if (angleToUp < 10 || angleToUp > 170)
                    {
                        ringNormal = Vector3.Cross(direction, Vector3.right);
                    }
                    else
                    {
                        ringNormal = Vector3.Cross(direction, Vector3.up);
                    }

                    ringNormal = ringNormal.normalized;

                    _prevNormal0 = ringNormal;
                }
                else if (_rings > 2)
                {
                    Vector3 prevPerp = Vector3.Cross(_prevRing0 - _prevRing1, _prevNormal0);
                    ringNormal = Vector3.Cross(prevPerp, ringPosition - _prevRing0).normalized;
                }

                if (_rings == 2)
                {
                    updateRingVerts(0,
                                    _prevRing0,
                                    ringPosition - _prevRing1,
                                    _prevNormal0,
                                    0);
                }

                if (_rings >= 2)
                {
                    updateRingVerts(_vertices.Count - _parent._drawResolution,
                                    ringPosition,
                                    ringPosition - _prevRing0,
                                    ringNormal,
                                    0);
                    updateRingVerts(_vertices.Count - _parent._drawResolution * 2,
                                    ringPosition,
                                    ringPosition - _prevRing0,
                                    ringNormal,
                                    1);
                    updateRingVerts(_vertices.Count - _parent._drawResolution * 3,
                                    _prevRing0,
                                    ringPosition - _prevRing1,
                                    _prevNormal0,
                                    1);
                }

                _prevRing1 = _prevRing0;
                _prevRing0 = ringPosition;

                _prevNormal0 = ringNormal;
            }

            private void addVertexRing()
            {
                for (int i = 0; i < _parent._drawResolution; i++)
                {
                    _vertices.Add(Vector3.zero);  //Dummy vertex, is updated later
                    _uvs.Add(new Vector2(i / (_parent._drawResolution - 1.0f), 0));
                    _colors.Add(_parent._drawColor);
                }
            }

            //Connects the most recently added vertex ring to the one before it
            private void addTriSegment()
            {
                for (int i = 0; i < _parent._drawResolution; i++)
                {
                    int i0 = _vertices.Count - 1 - i;
                    int i1 = _vertices.Count - 1 - ((i + 1) % _parent._drawResolution);

                    _tris.Add(i0);
                    _tris.Add(i1 - _parent._drawResolution);
                    _tris.Add(i0 - _parent._drawResolution);

                    _tris.Add(i0);
                    _tris.Add(i1);
                    _tris.Add(i1 - _parent._drawResolution);
                }
            }

            private void updateRingVerts(int offset, Vector3 ringPosition, Vector3 direction, Vector3 normal, float radiusScale)
            {
                direction = direction.normalized;
                normal = normal.normalized;

                for (int i = 0; i < _parent._drawResolution; i++)
                {
                    float angle = 360.0f * (i / (float)(_parent._drawResolution));
                    Quaternion rotator = Quaternion.AngleAxis(angle, direction);
                    Vector3 ringSpoke = rotator * normal * _parent._drawRadius * radiusScale;
                    _vertices[offset + i] = ringPosition + ringSpoke;
                }
            }
            //user defined
            public void deleteLine()
            {
                //_mesh.ClearBlendShapes();
            }
        }
    }
}
