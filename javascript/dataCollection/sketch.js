// ml5.js: Pose Classification
// The Coding Train / Daniel Shiffman
// https://thecodingtrain.com/Courses/ml5-beginners-guide/7.2-pose-classification.html
// https://youtu.be/FYgYyq-xqAw

// All code: https://editor.p5js.org/codingtrain/sketches/JoZl-QRPK

// Separated into three sketches
// 1: Data Collection: https://editor.p5js.org/codingtrain/sketches/kTM0Gm-1q
// 2: Model Training: https://editor.p5js.org/codingtrain/sketches/-Ywq20rM9
// 3: Model Deployment: https://editor.p5js.org/codingtrain/sketches/c5sDNr8eM

let video;
let poseNet;
let pose;
let skeleton;

let poseSet = false;
let pose1;
let pose2;
let pose3;

let noHead = false;
let noArms = false;

let brain;

let state = 'waiting';
let targeLabel;

function keyPressed() {
 if (key == 's') {
    brain.saveData();
  } else if (key == 'h') {
    noHead = !noHead;
    console.log("noHead" + noHead);
  } else if (key == 'a') {
    noArms = !noArms;
    console.log("noArms" + noArms);
  } else {
    targetLabel = key;
    console.log(targetLabel);
    setTimeout(function() {
      console.log('collecting');
      state = 'collecting';
      setTimeout(function() {
        console.log('not collecting');
        state = 'waiting';
      }, 20000);
    }, 10000);
  }
}

function setup() {
  createCanvas(640, 480);

  video = createCapture(VIDEO);
  video.hide();
  poseNet = ml5.poseNet(video, modelLoaded);
  poseNet.on('pose', gotPoses);

  let options = {
    inputs: 66,
    outputs: 8,
    task: 'classification',
    debug: true
  }
  brain = ml5.neuralNetwork(options);
}

function gotPoses(poses) {

  // console.log(poses);
  if (poses.length > 0) {
    pose = poses[0].pose;
    skeleton = poses[0].skeleton;

    if (!poseSet) {
      pose1 = pose;
      pose2 = pose;
      pose3 = pose;
      poseSet = true;
    } else {
      pose1 = pose2;
      pose2 = pose3;
      pose3 = pose;
    }


    if (state == 'collecting') {
      let inputs = [];
      for (let i = 0; i < pose.keypoints.length - 6; i++) {
        let x1 = pose1.keypoints[i].position.x;
        let y1 = pose1.keypoints[i].position.y;

        let x2 = pose2.keypoints[i].position.x;
        let y2 = pose2.keypoints[i].position.y;

        let x3 = pose3.keypoints[i].position.x;
        let y3 = pose3.keypoints[i].position.y;

        if (noArms && i > 4) {
          x1 = Math.floor(Math.random() * 640);
          y1 = Math.floor(Math.random() * 480);

          x2 = Math.floor(Math.random() * 640);
          y2 = Math.floor(Math.random() * 480);

          x3 = Math.floor(Math.random() * 640);
          y3 = Math.floor(Math.random() * 480);
        }

        if (noHead && i < 5) {
          x1 = Math.floor(Math.random() * 640);
          y1 = Math.floor(Math.random() * 480);

          x2 = Math.floor(Math.random() * 640);
          y2 = Math.floor(Math.random() * 480);

          x3 = Math.floor(Math.random() * 640);
          y3 = Math.floor(Math.random() * 480);
        }

        inputs.push(x1);
        inputs.push(y1);
        inputs.push(x2);
        inputs.push(y2);
        inputs.push(x3);
        inputs.push(y3);
      }
      let target = [targetLabel];
      brain.addData(inputs, target);
    }
  }
}


function modelLoaded() {
  console.log('poseNet ready');
}

function draw() {
  // background(255);
  translate(video.width, 0);
  scale(-1, 1);
  image(video, 0, 0, video.width, video.height);

  if (pose) {
    for (let i = 0; i < skeleton.length; i++) {
      let a = skeleton[i][0];
      let b = skeleton[i][1];
      strokeWeight(2);
      stroke(0);

      line(a.position.x, a.position.y, b.position.x, b.position.y);
    }
    for (let i = 0; i < pose.keypoints.length; i++) {
      let x1 = pose1.keypoints[i].position.x;
      let y1 = pose1.keypoints[i].position.y;
      fill(0);
      stroke(255);
      ellipse(x1, y1, 16, 16);

      let x2 = pose2.keypoints[i].position.x;
      let y2 = pose2.keypoints[i].position.y;
      fill(0);
      stroke(180);
      ellipse(x2, y2, 16, 16);

      line (x1, y1, x2, y2);

      let x3 = pose3.keypoints[i].position.x;
      let y3 = pose3.keypoints[i].position.y;
      fill(0);
      stroke(100);
      ellipse(x3, y3, 16, 16);

      line (x2, y2, x3, y3);

    }
  }
}
