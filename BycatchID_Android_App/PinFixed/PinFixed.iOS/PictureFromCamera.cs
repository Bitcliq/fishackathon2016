using AssetsLibrary;
using AVFoundation;
using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(PinFixed.PictureFromCam), typeof(PinFixed.iOS.PictureFromCamera))]
namespace PinFixed.iOS
{
    // Renderer for Picture
    public class PictureFromCamera : PageRenderer
    {
        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureStillImageOutput stillImageOutput;
        UIView liveCameraStream;
        UIButton takePhotoButton;
        UIButton toggleCameraButton;
        UIButton toggleFlashButton;


        UIImagePickerController imagePicker;
        UIButton choosePhotoButton;
        UIImageView imageView;

        byte[] imageBytes;


        TakePicScreen _page;

        UIWindow window;
        UIViewController viewController;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            _page = e.NewElement as TakePicScreen;

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                AuthorizeCameraUse();
                SetupUserInterface();
                SetupEventHandlers();
             
                SetupLiveCameraStream();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }

       


        }

   



        void SetupUserInterface()
        {
            var centerButtonX = View.Bounds.GetMidX() - 35f;
            var topLeftX = View.Bounds.X + 25;
            var topRightX = View.Bounds.Right - 65;
            var bottomButtonY = View.Bounds.Bottom - 150;
            var topButtonY = View.Bounds.Top + 15;
            var buttonWidth = 70;
            var buttonHeight = 70;

           

            takePhotoButton = new UIButton()
            {
                Frame = new CGRect(centerButtonX, bottomButtonY, buttonWidth, buttonHeight),
          

            };



            takePhotoButton.BackgroundColor = UIColor.Blue;
            takePhotoButton.SetTitle ("Take Picture", UIControlState.Normal);
            View.Add(takePhotoButton);
        }


        void SetupEventHandlers()
        {
            takePhotoButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                CapturePhoto();
            };

          

        }

        async void CapturePhoto()
        {
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
            var jpegImage = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);

            var photo = new UIImage(jpegImage);

            float width = 1000;
            float height = 1000;
            using (CGImage imageRef = photo.CGImage)
            {
                CGImageAlphaInfo alphaInfo = imageRef.AlphaInfo;
                CGColorSpace colorSpaceInfo = CGColorSpace.CreateDeviceRGB();
                if (alphaInfo == CGImageAlphaInfo.None)
                {
                    alphaInfo = CGImageAlphaInfo.NoneSkipLast;
                }


                int maxSize = (int)width;

                width = imageRef.Width;
                height = imageRef.Height;


                if (height >= width)
                {
                    width = (int)Math.Floor((double)width * ((double)maxSize / (double)height));
                    height = maxSize;
                }
                else
                {
                    height = (int)Math.Floor((double)height * ((double)maxSize / (double)width));
                    width = maxSize;
                }


                CGBitmapContext bitmap;

                if (photo.Orientation == UIImageOrientation.Up || photo.Orientation == UIImageOrientation.Down)
                {
                    bitmap = new CGBitmapContext(IntPtr.Zero, (int)width, (int)height, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
                }
                else
                {
                    bitmap = new CGBitmapContext(IntPtr.Zero, (int)height, (int)width, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
                }

                try
                {
                    switch (photo.Orientation)
                    {
                        case UIImageOrientation.Left:
                            bitmap.RotateCTM((float)Math.PI / 2);
                            bitmap.TranslateCTM(0, -height);
                            break;
                        case UIImageOrientation.Right:
                            bitmap.RotateCTM(-((float)Math.PI / 2));
                            bitmap.TranslateCTM(-width, 0);
                            break;
                        case UIImageOrientation.Up:
                            break;
                        case UIImageOrientation.Down:
                            bitmap.TranslateCTM(width, height);
                            bitmap.RotateCTM(-(float)Math.PI);
                            break;
                    }
                }
                catch (Exception)
                {
                }

                bitmap.DrawImage(new System.Drawing.Rectangle(0, 0, (int)width, (int)height), imageRef);



                UIImage res = UIImage.FromImage(bitmap.ToImage());
                bitmap = null;

                imageBytes = res.AsJPEG().ToArray();

                imagePicker.DismissModalViewController(true);
                _page.SetCoordinatesAndImage(imageBytes, "", "", "", "");

            }

        }

  

       

  


        void SetupLiveCameraStream()
        {
            captureSession = new AVCaptureSession();

            var viewLayer = liveCameraStream.Layer;
            var videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
            {
                Frame = liveCameraStream.Bounds
            };
            liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

            var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            captureSession.AddOutput(stillImageOutput);
            captureSession.AddInput(captureDeviceInput);
            captureSession.StartRunning();
        }

        void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }

        async void AuthorizeCameraUse()
        {
            var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
            if (authorizationStatus != AVAuthorizationStatus.Authorized)
            {
                await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
            }
        }
    }
}

