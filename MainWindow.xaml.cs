using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;


namespace _11._02._23_WPF_MajorProject_MonsterViewers {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        //DEFINE MONSTER STRUCT
        struct Monster {
            public int MonsterID;
            public string MonsterName;
            public int HealthPoints;
            public int Gold;
            public int Experience;
            public int PhysicalPower;
            public int MagicalPower;
            public int PhysicalDefense;
            public int MagicalDefense;
            public string Family;
            public string WeakAgainst;
            public string StrongAgainst;
        }//end struct Monster

        //GLOBAL VARIABLE TO HOLD PERSON DATA
        Monster[] globalMonsterData;

        public MainWindow() {
            InitializeComponent();

            loadData();
        }//end main

        #region Events
        private void loadData() {
            string filePath = "C:\\Users\\MCA\\Documents\\Documents_Vinh\\WPF applications\\Major Project_Monsters\\monster.csv";

            //GET RECORD COUNT
            int records = CountCsvRecords(filePath, true);

            //SET SLIDER TO MATCH
            sliderMonster.Maximum = records - 1;

            //LOAD DATA FROM CSV & RETURN ARRAY OF PERSON
            globalMonsterData = ProcessCsvDataIntoMonsterStruct(filePath, records, true);

            //UPDATE FORM WITH 1ST PERSON'S DATA
            UpdateForm(0);
            imageFile();

        }//end loadData()
        private void buttonRightNext(object sender, RoutedEventArgs e) {
 
            //CREATE AN INT TO SNAP TO RIGHTNEXT BUTTON
            int RightButtonInt = (int)sliderMonster.Value;

            //Click to Next
            sliderMonster.Value = RightButtonInt + 1;

        }//end buttonRightNext

        private void buttonLeftPrev(object sender, RoutedEventArgs e) {

            //CREATE AN INT TO SNAP TO RIGHTNEXT BUTTON
            int LeftButtonInt = (int)sliderMonster.Value;

            //Click to Next
            sliderMonster.Value = LeftButtonInt - 1;

        }//end buttonLeftPrev

        private void buttonRightLastRecord(object sender, RoutedEventArgs e) {

            //CREATE AN INT TO SNAP TO RIGHTNEXT BUTTON
            int RightButtonLastRecordInt = (int)sliderMonster.Value;

            //Click to Next
            sliderMonster.Value = RightButtonLastRecordInt;
            sliderMonster.Value = sliderMonster.Maximum;

        }//end buttonRightLastRecord


        private void buttonLeftBeginningRecord(object sender, RoutedEventArgs e) {

            //CREATE AN INT TO SNAP TO RIGHTNEXT BUTTON
            int LeftButtonBeginningRecordInt = (int)sliderMonster.Value;

            //Click to Next
            sliderMonster.Value = LeftButtonBeginningRecordInt;
            sliderMonster.Value = sliderMonster.Minimum;

        }//end buttonleftBeginningRecord

        private void slideRecord_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            //CHECK IF NO DATA HAS BEEN LOADED EXIT IF THIS IS THE CASE
            if (globalMonsterData == null) {
                sliderMonster.Value = 0;
                return;
            }//end if

            //CREATE AN INT TO SNAP SLIDER VALUE TO
            int sliderInt = (int)sliderMonster.Value;

            //UPDATE LABEL SHOWIN THE CURRENTLY SELECTED RECORD
            txtBoxMonsterIDSlider.Text = sliderInt.ToString();

            //UPDATE FORM
            UpdateForm(sliderInt);
            imageFile();
        }//end event

        //--------Images----------

        private void imageFile() {
            string imageFilePath = $"C:\\Users\\MCA\\Documents\\Documents_Vinh\\WPF applications\\Major Project_Monsters\\images\\{txtBoxMonsterName.Text}.png";
            
            LoadImage(imgboxMonster, imageFilePath);
        }//end imageFile


        void LoadImage(Image imgTarget, string imageFilePath) {
            //SAFETY CHECK FILE FIRST
            string noImageFile = "C:\\Users\\MCA\\Documents\\Documents_Vinh\\WPF applications\\Major Project_Monsters\\images\\no_image_available.png";

            if (File.Exists(imageFilePath) == false) {
                imageFilePath = noImageFile;
                
            }//end if

            //CREATE A BITMAP
            BitmapImage bmpImage = new BitmapImage();

            //SET BITMAP FOR EDITING
            bmpImage.BeginInit();
            bmpImage.UriSource = new Uri(imageFilePath);//LOAD THE IMAGE

            bmpImage.EndInit();

            //SET THE SOURCE OF THE IMAGE CONTROL TO THE BITMAP
            imgTarget.Source = bmpImage;
            

        }//end LoadImage()


        #endregion


        #region Functions

        int CountCsvRecords(string filePath, bool skipHeader) {
            //VARS
            int recordCount = 0;

            //OPEN THE FILE TO COUNT THE NUMBER OF RECORDS
            StreamReader infile = new StreamReader(filePath);

            //CONSUME HEADER WITH A READLINE
            if (skipHeader) {
                infile.ReadLine();
            }//end if

            //COUNT RECORDS
            while (infile.EndOfStream == false) {
                infile.ReadLine();
                recordCount += 1;
            }//end whle 

            //CLOSE FILE
            infile.Close();

            return recordCount;
        }//end function

        Monster[] ProcessCsvDataIntoMonsterStruct(string filePath, int recordsToRead, bool skipHeader) {
            //VARS
            Monster[] returnArray = new Monster[recordsToRead];
            int currentRecordCount = 0;

            //OPEN THE FILE TO COUNT THE NUMBER OF RECORDS
            StreamReader infile = new StreamReader(filePath);

            //CONSUME HEADER WITH A READLINE
            if (skipHeader) {
                infile.ReadLine();
            }//end if

            //COUNT RECORDS
            while (infile.EndOfStream == false && currentRecordCount < recordsToRead) {
                string record = infile.ReadLine();
                string[] fields = record.Split(",");


                for (int i = 0; i < fields.Length; i++) {
                    returnArray[currentRecordCount].MonsterID = currentRecordCount;
                    if (fields[i] == ""|| fields[i] == "?") {
                        

                    } else if (fields[i] != ""&& fields[i] == fields[0]) {
                        returnArray[currentRecordCount].MonsterName = fields[0];
                    } else if (fields[i] != "" && fields[i] == fields[1]) {
                        returnArray[currentRecordCount].HealthPoints = int.Parse(fields[1]);
                    } else if (fields[i] != "" && fields[i] == fields[2]) {
                        returnArray[currentRecordCount].Gold = int.Parse(fields[2]);
                    } else if (fields[i] != "" && fields[i] == fields[3]) {
                        returnArray[currentRecordCount].Experience = int.Parse(fields[3]);
                    } else if (fields[i] != "" && fields[i] == fields[4]) {
                        returnArray[currentRecordCount].PhysicalPower = int.Parse(fields[4]);
                    } else if (fields[i] != "" && fields[i] == fields[5]) {
                        returnArray[currentRecordCount].MagicalPower = int.Parse(fields[5]);
                    } else if (fields[i] != "" && fields[i] == fields[6]) {
                        returnArray[currentRecordCount].PhysicalDefense = int.Parse(fields[6]);
                    } else if (fields[i] != "" && fields[i] == fields[7]) {
                        returnArray[currentRecordCount].MagicalDefense = int.Parse(fields[7]);
                    } else if (fields[i] != "" && fields[i] == fields[8]) {
                        returnArray[currentRecordCount].Family = fields[8];
                    } else if (fields[i] != "" && fields[i] == fields[9]) {
                        returnArray[currentRecordCount].WeakAgainst = fields[9];
                    } else if (fields[i] != "" && fields[i] == fields[10]) {
                        returnArray[currentRecordCount].StrongAgainst = fields[10];
                    } else {
                        i++;
                    }//

                }//for
                currentRecordCount += 1;

                //-----TRYPARSE------
                /*int paresedValue;
                 bool canParse = int.TryParse(fields[5], out returnArray[currentRecordCount].MagicalPower);

                 returnArray[currentRecordCount].MonsterID = currentRecordCount;
                 returnArray[currentRecordCount].MonsterName = fields[0];
                 returnArray[currentRecordCount].HealthPoints = int.Parse(fields[1]);
                 returnArray[currentRecordCount].Gold = int.Parse(fields[2]);
                 returnArray[currentRecordCount].Experience = int.Parse(fields[3]);
                 returnArray[currentRecordCount].PhysicalPower = int.Parse(fields[4]);
                 returnArray[currentRecordCount].MagicalPower = int.Parse(fields[5]);
                 returnArray[currentRecordCount].PhysicalDefense = int.Parse(fields[6]);
                 returnArray[currentRecordCount].MagicalDefense = int.Parse(fields[7]);
                 returnArray[currentRecordCount].Family = fields[8];
                 returnArray[currentRecordCount].WeakAgainst = fields[9];
                 returnArray[currentRecordCount].StrongAgainst = fields[10];

                 currentRecordCount += 1;*/


            }//end whle 

            //CLOSE FILE
            infile.Close();

            return returnArray;
        }//end function

        void UpdateForm(int MonsterIndex) {
            //GRAB PERSON FROM THE GLOBAL ARRAY
            Monster currentMonster = globalMonsterData[MonsterIndex];

            //UPDATE TEXBOXES ON THE FORM

            txtBoxMonsterIDSlider.Text = currentMonster.MonsterID.ToString();
            txtBoxMonsterIDheader.Text = currentMonster.MonsterID.ToString();

            txtBoxMonsterName.Text = currentMonster.MonsterName;
            txtBoxHealthPoint.Text = currentMonster.HealthPoints.ToString();
            txtBoxGold.Text = currentMonster.Gold.ToString();
            txtBoxExperience.Text = currentMonster.Experience.ToString();
            txtBoxPhysicalPower.Text = currentMonster.PhysicalPower.ToString();
            txtBoxMagicalPower.Text = currentMonster.MagicalPower.ToString();
            txtBoxPhysicalDefense.Text = currentMonster.PhysicalDefense.ToString();
            txtBoxMagicalDefense.Text = currentMonster.MagicalDefense.ToString();
            txtBoxFamily.Text = currentMonster.Family;
            txtBoxWeakAgainst.Text = currentMonster.WeakAgainst;
            txtBoxStrongAgainst.Text = currentMonster.StrongAgainst;
        }//end function 

        #endregion



    }//end class
}//end namespace
