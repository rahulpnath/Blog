using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using SimpleGPS.RouteServiceReference;
using System.Collections.ObjectModel;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Phone.Shell;

namespace SimpleGPS
{
    public partial class MainPage : PhoneApplicationPage
    {
        double latitude, longitude;
        public static string speechUrl = "http://translate.google.com/translate_tts?tl=en&q=";
        private GeoCoordinate currentLocation;
        private Pushpin startingPoint;
        private Pushpin destination;
        ObservableCollection<string> routeText;
        private bool isInDriveMode = false;
        ObservableCollection<String> AvailableDestinationsOptions;

        string bingAppId = "hlcpUmABrhEiaVJkHpaOzyUqpLqftgp+67349jRehNw=";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            mapBing.ZoomBarVisibility = System.Windows.Visibility.Visible;
            routeText = new ObservableCollection<string>();
            AvailableDestinationsOptions = new ObservableCollection<String>();
            drivingDirectionsList.ItemsSource = routeText;
            SetDriveMode(isInDriveMode);
            availableDestinations.ItemsSource = AvailableDestinationsOptions;
            TrackMe();
        }

        GeoCoordinateWatcher myWatcher;

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            TrackMe();
            
        }

        GeoPosition<GeoCoordinate> myPosition;

        private void TrackMe()
        {
            startingPoint = null;
            CurrentPosition.Children.Clear();
            if (myWatcher != null)
            {
                myWatcher.PositionChanged -= myWatcher_PositionChanged;
                myWatcher.Dispose();
                myWatcher = null;
            }
            myWatcher = new GeoCoordinateWatcher();
            myWatcher.TryStart(false, TimeSpan.FromMilliseconds(1000));
            myWatcher.PositionChanged += myWatcher_PositionChanged;
        }

        void myWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
             myPosition = myWatcher.Position;
             currentLocation = myWatcher.Position.Location;
             if (isInDriveMode)
             {
                 DrawMyCurrentRoute(currentLocation);
             }

             LocationManager.GetLocationName(UpdateLocation, myPosition.Location.Latitude.ToString(), myPosition.Location.Longitude.ToString());
        }

        void UpdateLocation(String message)
        {
            if (!myPosition.Location.IsUnknown)
            {
                latitude = myPosition.Location.Latitude;
                longitude = myPosition.Location.Longitude;
                Pushpin pin = new Pushpin();
                pin.Content = message;
                pin.Location = myPosition.Location;
                
                if (!isInDriveMode || startingPoint == null || startingPoint.Location == myPosition.Location)
                {
                    startingPoint = pin;
                    StartPoint.Children.Clear();
                    StartPoint.Children.Add(pin);
                }
                else
                {
                    // this is a realtime location change update
                    CurrentPosition.Children.Clear();
                    pin.Background = new SolidColorBrush(Colors.Yellow);
                    CurrentPosition.Children.Add(pin);
                }
                mapBing.Center = pin.Location;
            }

            mapBing.Center = new GeoCoordinate(latitude, longitude);
            mapBing.ZoomLevel = 10;
        }

        private void UpdateSearchLocation(SimpleGPS.LocationQueryResponse.ResourceSet[] resourceSets)
        {
            if (resourceSets != null)
            {
                DestinationPoint.Children.Clear();
                AvailableDestinationsOptions.Clear();

                foreach (var resourceSet in resourceSets)
                {
                    foreach(var resource in resourceSet.resources)
                    {
                        // add only unique names in the list
                        if (!AvailableDestinationsOptions.Contains(resource.name))
                        {
                            var latitude = double.Parse(resource.point.coordinates[0]);
                            var longitude = double.Parse(resource.point.coordinates[1]);

                            Pushpin pin = new Pushpin();
                            pin.Content = resource.name;
                            pin.Location = new GeoCoordinate(latitude, longitude);
                            pin.Background = new SolidColorBrush(Colors.Red);
                            DestinationPoint.Children.Add(pin);
                            AvailableDestinationsOptions.Add(resource.name);
                            pin.Tap += pin_Tap;
                        }
                    }
                }
            }
        }

        void pin_Tap(object sender1, System.Windows.Input.GestureEventArgs e1)
        {
            List<GeoCoordinate> locations = new List<GeoCoordinate>();
            locations.Add(currentLocation);
            Pushpin senderAs = sender1 as Pushpin;
            locations.Add(senderAs.Location);
            destination = senderAs;
            DestinationPoint.Children.Clear();
            DestinationPoint.Children.Add(destination);

            RouteServiceClient routeService = new RouteServiceClient("BasicHttpBinding_IRouteService");

            routeService.CalculateRouteCompleted += (sender, e) =>
            {
                DrawRoute(e);
            };

            mapBing.SetView(LocationRect.CreateLocationRect(locations));

            routeService.CalculateRouteAsync(new RouteRequest()
            {
                Credentials = new Credentials()
                {
                    ApplicationId = LocationManager.bingApiKey
                },
                Options = new RouteOptions()
                {
                    RoutePathType = RoutePathType.Points
                },
                Waypoints = new ObservableCollection<Waypoint>(
                    locations.Select(x => new Waypoint()
                    { 
                        Location = new Microsoft.Phone.Controls.Maps.Platform.Location() { Latitude = x.Latitude, Longitude = x.Longitude }
                    }))
            });
            SetDriveMode(true);
        }

        private void SetDriveMode(bool isDrive)
        {
            isInDriveMode = isDrive;
            if (isDrive)
            {
                // hide the destinations list
                LocationDetails.Visibility = System.Windows.Visibility.Collapsed;
                drivingDirections.Visibility = System.Windows.Visibility.Visible;
                directionsPanorma.Header = "Directions";
            }
            else
            {
                directionsPanorma.Header = "Destinations";
                LocationDetails.Visibility = System.Windows.Visibility.Visible;
                drivingDirections.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void GetTextDetailsOfRoute(RouteLeg leg)
        {
            routeText.Clear();
            foreach (var itenary in leg.Itinerary)
            {
                Regex regex = new Regex("<[a-zA-z:@#$%^&*()~+?\\/0-9]*>");
                var text = regex.Replace(itenary.Text, " ");
                
                routeText.Add(text.TrimEnd().TrimStart());
            }
        }

        private void DrawRoute(CalculateRouteCompletedEventArgs e)
        {
            var points = e.Result.Result.RoutePath.Points;

            GetTextDetailsOfRoute(e.Result.Result.Legs[0]);

           SpeakText(routeText[0]);
            var coordinates = points.Select(x => new GeoCoordinate(x.Latitude, x.Longitude));

            var routeColor = Colors.Blue;
            var routeBrush = new SolidColorBrush(routeColor);

            var routeLine = new MapPolyline()
            {
                Locations = new LocationCollection(),
                Stroke = routeBrush,
                Opacity = 0.65,
                StrokeThickness = 5.0,
            };

            foreach (var location in points)
            {
                routeLine.Locations.Add(location);
            }
           
            RouteLayer.Children.Add(routeLine);
        }


        private void DrawMyCurrentRoute(GeoCoordinate location)
        {
            MapPolyline polyline;
            if (MyPathLayer.Children.Count == 0)
            {

                polyline = new MapPolyline()
                {
                    Locations = new LocationCollection(),
                    Stroke = new SolidColorBrush(Colors.Green),
                    Opacity = 0.65,
                    StrokeThickness = 5.0,
                };

                MyPathLayer.Children.Add(polyline);

            }
            else
            {
                polyline = MyPathLayer.Children[0] as MapPolyline;
            }
            polyline.Locations.Add(location);
        }


        private void SpeakText(string message)
        {
            // Play the audio
            med1.Source = new Uri(speechUrl + message);
            med1.Play(); 
        }

        void translator_SpeakCompleted(object sender, TTSService.SpeakCompletedEventArgs e)
        {
            var client = new WebClient();
            client.OpenReadCompleted += ((s, args) =>
            {
                SoundEffect se = SoundEffect.FromStream(args.Result);
                se.Play();
            });
            client.OpenReadAsync(new Uri(e.Result));
        }

        private void preventAutoLock_Checked_1(object sender, RoutedEventArgs e)
        {
            // Disable auto locking of the phone
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        private void preventAutoLock_Unchecked_1(object sender, RoutedEventArgs e)
        {
            // Enable Auto Locking of the phone
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

        private void destinationLocation_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Search for the entered location
                LocationManager.GetLocationCordinates(UpdateSearchLocation, destinationLocation.Text);
            }
        }

        private void availableDestinations_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var selection = availableDestinations.SelectedItem as String;
            if (selection != null)
            {
                // get the corresponding pin 
                var pin = DestinationPoint.Children.Cast<Pushpin>().FirstOrDefault(a => a.Content.ToString() == selection);
                pin_Tap(pin, null);
            }
        }

        private void clearMap_Click_1(object sender, RoutedEventArgs e)
        {
            // clear the map contents 
            destinationLocation.Text = String.Empty;
            AvailableDestinationsOptions.Clear();
            DestinationPoint.Children.Clear();
            CurrentPosition.Children.Clear();
            StartPoint.Children.Clear();
            RouteLayer.Children.Clear();
            MyPathLayer.Children.Clear();
            routeText.Clear();
            SetDriveMode(false);
            TrackMe();
        }

    }
}