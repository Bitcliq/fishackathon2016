using AssetsLibrary;
using AVFoundation;
using CoreGraphics;
using CustomRenderer;
using CustomRenderer.iOS;
using Foundation;
using ImageIO;
using PinFixed;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PinFixed.TakePicScreen), typeof(CameraPageRenderer))]
namespace CustomRenderer.iOS
{
    public class CameraPageRenderer : PageRenderer
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
                //SetupUserInterface();
                //SetupEventHandlers();
                //SetupLiveCameraStream ();
                AuthorizeCameraUse();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
            }

            //choosePhotoButton.SendActionForControlEvents(UIControlEvent.TouchUpInside);

        }



        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();

            // create a new picker controller
            imagePicker = new UIImagePickerController();

            // set our source to the photo library
            imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

            // set what media types
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
            imagePicker.Canceled += Handle_Canceled;


            window = UIApplication.SharedApplication.KeyWindow;
            viewController = window.RootViewController;

            if (viewController == null)
            {
                while (viewController.PresentedViewController != null)
                    viewController = viewController.PresentedViewController;
                await viewController.PresentViewControllerAsync(imagePicker, true);
            }
            else
                await viewController.PresentViewControllerAsync(imagePicker, true);



            // show the picker
            //NavigationController.PresentModalViewController(imagePicker, true);
            //UIPopoverController picc = new UIPopoverController(imagePicker);

        }
        void SetupUserInterface()
        {
            var centerButtonX = View.Bounds.GetMidX() - 35f;
            this.View.InsertSubview(new UIImageView(UIImage.FromBundle("bg_home_sintra.png")), 0);
        

            var topLeftX = View.Bounds.X + 25;
            var topRightX = View.Bounds.Right - 65;
            var bottomButtonY = View.Bounds.Bottom - 150;
            var topButtonY = View.Bounds.Top + 15;
            var buttonWidth = 100;
            var buttonHeight = 70;

       

            
            choosePhotoButton = UIButton.FromType(UIButtonType.RoundedRect);
            choosePhotoButton.BackgroundColor = UIColor.White;

            choosePhotoButton.Frame = new CGRect(centerButtonX, 150, buttonWidth, buttonHeight);
            choosePhotoButton.SetTitle("Biblioteca", UIControlState.Normal);



            choosePhotoButton.TouchUpInside += (s, e) =>
            {
                OpenImgPick();

            };
            View.Add(choosePhotoButton);
   

        }


        void OpenImgPick()
        {
    
            
        }



        protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
        {
            // determine what was selected, video or image
            bool isImage = false;
            switch (e.Info[UIImagePickerController.MediaType].ToString())
            {
                case "public.image":


                    isImage = true;
                    break;
                case "public.video":
                   

                    break;
            }
          

            // get common info (shared between images and video)
            NSUrl referenceURL = e.Info[UIImagePickerController.ReferenceUrl] as NSUrl;


            if (isImage)
            {
                ALAssetsLibrary assetsLibrary = new ALAssetsLibrary();

                float width = 1000;
                float height = 1000;
                assetsLibrary.AssetForUrl(referenceURL, delegate(ALAsset asset)
                {

                    ALAssetRepresentation representation = asset.DefaultRepresentation;

                    string latitude = null, longitude = null, latitudeRef = null, longitudeRef = null;

                    if (asset.DefaultRepresentation.Metadata != null)
                    {
                        NSDictionary dic = asset.DefaultRepresentation.Metadata;


                        try
                        {



                            var gpsDic = (NSDictionary)dic[new NSString("{GPS}")];

                            latitude = gpsDic[new NSString("Latitude")] + "";
                            longitude = gpsDic[new NSString("Longitude")] + "";
                            latitudeRef = gpsDic[new NSString("LatitudeRef")] + "";
                            longitudeRef = gpsDic[new NSString("LongitudeRef")] + "";
                        }
                        catch (Exception)
                        {
                        }
                    }


                    UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;




                    using (CGImage imageRef = originalImage.CGImage)
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

                        if (originalImage.Orientation == UIImageOrientation.Up || originalImage.Orientation == UIImageOrientation.Down)
                        {
                            bitmap = new CGBitmapContext(IntPtr.Zero, (int)width, (int)height, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
                        }
                        else
                        {
                            bitmap = new CGBitmapContext(IntPtr.Zero, (int)height, (int)width, imageRef.BitsPerComponent, imageRef.BytesPerRow, colorSpaceInfo, alphaInfo);
                        }

                        try
                        {
                            switch (originalImage.Orientation)
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
                        _page.SetCoordinatesAndImage(imageBytes, latitude, latitudeRef, longitude, longitudeRef);

                    }

                }, delegate(NSError error)
                {

                    Console.WriteLine("User denied access to photo Library... {0}", error);


                });

            }

          




        }


        void Handle_Canceled(object sender, EventArgs e)
        {
            //Console.WriteLine("picker cancelled");
            imagePicker.DismissModalViewController(true);
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

        void ToggleFrontBackCamera()
        {
            var devicePosition = captureDeviceInput.Device.Position;
            if (devicePosition == AVCaptureDevicePosition.Front)
            {
                devicePosition = AVCaptureDevicePosition.Back;
            }
            else
            {
                devicePosition = AVCaptureDevicePosition.Front;
            }

            var device = GetCameraForOrientation(devicePosition);
            ConfigureCameraForDevice(device);

            captureSession.BeginConfiguration();
            captureSession.RemoveInput(captureDeviceInput);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
            captureSession.AddInput(captureDeviceInput);
            captureSession.CommitConfiguration();
        }

        void ToggleFlash()
        {
            var device = captureDeviceInput.Device;

            var error = new NSError();
            if (device.HasFlash)
            {
                if (device.FlashMode == AVCaptureFlashMode.On)
                {
                    device.LockForConfiguration(out error);
                    device.FlashMode = AVCaptureFlashMode.Off;
                    device.UnlockForConfiguration();
                    toggleFlashButton.SetBackgroundImage(UIImage.FromFile("NoFlashButton.png"), UIControlState.Normal);
                }
                else
                {
                    device.LockForConfiguration(out error);
                    device.FlashMode = AVCaptureFlashMode.On;
                    device.UnlockForConfiguration();
                    toggleFlashButton.SetBackgroundImage(UIImage.FromFile("FlashButton.png"), UIControlState.Normal);
                }
            }
        }

        AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
        {
            var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

            foreach (var device in devices)
            {
                if (device.Position == orientation)
                {
                    return device;
                }
            }
            return null;
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

