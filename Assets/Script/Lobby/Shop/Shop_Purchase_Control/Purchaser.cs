using System;
using Ad;
using Alarm;
using Timer;
using UnityEngine;
using UnityEngine.Purchasing;


namespace Shop
{
    // Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
    public  class Purchaser : MonoBehaviour, IStoreListener
    {
        private static IStoreController m_StoreController;          // The Unity Purchasing system.
        private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

        private static Gem_Controller gem_controller;
        private static Shop_Avatar_Package _avatarPackage;
        // Product identifiers for all products capable of being purchased: 
        // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
        // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
        // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

        // General product identifiers for the consumable, non-consumable, and subscription products.
        // Use these handles in the code to reference which product to purchase. Also use these values 
        // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
        // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
        // specific mapping to Unity Purchasing's AddProduct, below.
        
        [Header("Commodity_Gem")]
        private static string GEM30 = "gem_30";
        private static string GEM70 = "gem_70";
        private static string GEM300 = "gem_300";
        private static string GEM700 = "gem_700";
        private static string GEM1500 = "gem_1500";
        private static string GEM3400 = "gem_3400";
        private static string Starter = "starter_pack";
        
        [Header("Package_Item")]
        private static string Noads = "no_ads";
        private static string Astronaut = "astronaut";
        private static string Party = "party";
        private static string Bear = "bear";
        private static string Science = "science";

        [Header("SuperSale_Item")] 
        private static string astro_sale = "astronaut_sale";
        private static string party_sale = "party_sale";
        private static string bear_sale = "bear_sale";
        private static string science_sale = "science_sale";

        private Panel_Control _panelControl;
        private IAlarmMediator _alarmMediator;
        private bool determine_restore = false;
        
        void Awake()
        {
            #if UNITY_ANDROID
            if (PlayerPrefs.GetInt("Restore", 0) == 0)
                determine_restore = true;
            #endif
            
            // If we haven't set up the Unity Purchasing reference
            if (m_StoreController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
            }
            
            gem_controller = new Gem_Controller();
            _avatarPackage = new Shop_Avatar_Package();
            _panelControl = GameObject.FindWithTag("Panel_Control").GetComponent<Panel_Control>();
        }

        public void OnClick_Shop()
        {
            #if UNITY_ANDROID
            if (PlayerPrefs.GetInt("Restore", 0) == 0)
            {
                PlayerPrefs.SetInt("Restore", 1); 
                determine_restore = false;
            }
            #endif
        }
        public void InitializePurchasing() 
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.

            #region Set_Product
            builder.AddProduct(GEM30, ProductType.Consumable);
            builder.AddProduct(GEM70, ProductType.Consumable);
            builder.AddProduct(GEM300, ProductType.Consumable);
            builder.AddProduct(GEM700, ProductType.Consumable);
            builder.AddProduct(GEM1500, ProductType.Consumable);
            builder.AddProduct(GEM3400, ProductType.Consumable);
            builder.AddProduct(Starter , ProductType.Consumable);
            
            // Continue adding the non-consumable product
            builder.AddProduct(Noads , ProductType.NonConsumable);
            builder.AddProduct(Astronaut , ProductType.NonConsumable);
            builder.AddProduct(Party , ProductType.NonConsumable);
            builder.AddProduct(Bear , ProductType.NonConsumable);
            builder.AddProduct(Science , ProductType.NonConsumable);

            // 세일 상품 등록하기 
            builder.AddProduct(astro_sale, ProductType.NonConsumable);
            builder.AddProduct(party_sale, ProductType.NonConsumable);
            builder.AddProduct(bear_sale, ProductType.NonConsumable);
            builder.AddProduct(science_sale, ProductType.NonConsumable);
            
            #endregion
            
            // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
            // if the Product ID was configured differently between Apple and Google stores. Also note that
            // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
            // must only be referenced here. 
            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }
        
        private bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return m_StoreController != null && m_StoreExtensionProvider != null;
        }

        /// <summary>
        /// CALLED WHEN USER CLICK NONCONSUMABLE OBJECT
        /// </summary>
        /// <param name="index"></param>
        public void BuyNonConsumable(int index)
        {
            switch (index)
            {

                case 1:
                    BuyProductID_NonConsumable(Noads);
                    break;
                
                case 3:
                    BuyProductID_NonConsumable(Astronaut);
                    break;
                
                case 2:
                    BuyProductID_NonConsumable(Party);
                    break;

                case 4:
                    BuyProductID_NonConsumable(Bear);
                    break;
                
                case 5:
                    BuyProductID_NonConsumable(Science);
                    break;
            }
            
        }
        
        /// <summary>
        /// CALLED WHEN USER CLICK CONSUMABLE OBJECT
        /// </summary>
        /// <param name="index"></param>
        public void BuyConsumable(int index)
        {
            
            switch (index)
            {
                default:
                    BuyProductID_Consumable(GEM30);
                    break;
                
                case 1:
                    BuyProductID_Consumable(GEM70);
                    break;
                
                case 2:
                    BuyProductID_Consumable(GEM300);
                    break;                
                
                case 3:
                    BuyProductID_Consumable(GEM700);
                    break;               
                
                case 4:
                    BuyProductID_Consumable(GEM1500);
                    break;   
                
                case 5:
                    BuyProductID_Consumable(GEM3400);
                    break;
                
                case 6:
                    BuyProductID_Consumable(Starter);
                    break;
                
            }
        }

        public void BuySaleProduct(int index)
        {
            switch (index)
            {
                case 0:
                    BuyProductID_NonConsumable(party_sale);
                    break;
                
                case 1:
                    BuyProductID_NonConsumable(astro_sale);
                    break;
                
                case 2:
                    BuyProductID_NonConsumable(bear_sale);
                    break;
                
                case 3: 
                    BuyProductID_NonConsumable(science_sale);
                    break;
            }
        }
        
        void BuyProductID_Consumable(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        
        void BuyProductID_NonConsumable(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                Product product = m_StoreController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        
        
        
        // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
        // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
        public void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ... 
            if (Application.platform == RuntimePlatform.IPhonePlayer || 
                Application.platform == RuntimePlatform.OSXPlayer)
            {
                // ... begin restoring purchases
                Debug.Log("RestorePurchases started ...");
                // Fetch the Apple store-specific subsystem.
                var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
                // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
                // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
                apple.RestoreTransactions((result) => {
                    // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                    // no purchases are available to be restored.
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                    // 복구 완료 
                    if (result)
                    {
                        GameObject okPanel = GameObject.FindWithTag("restore_panel").transform.GetChild(0).gameObject;
                        okPanel.SetActive(true);
                    }

                    else
                    {
                        GameObject okPanel = GameObject.FindWithTag("restore_panel").transform.GetChild(1).gameObject;
                        okPanel.SetActive(true);
                    }
                });
            }
            // Otherwise ...
            else
            {
                // We are not running on an Apple device. No work is necessary to restore purchases.
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            }
        }
        
        
        //  
        // --- IStoreListener
        //
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            m_StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            m_StoreExtensionProvider = extensions;
            Get_Price.Set_Store_Controller(ref controller);
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
        {
            if (!determine_restore)
            {
                FirstPurchase_Timer.First_Purchase(args.purchasedProduct.definition.id);
            }

            else
            {
                FirstPurchase_Timer.Restore_Purchase();
            }
            // A consumable product has been purchased by this user.
            if (String.Equals(args.purchasedProduct.definition.id, GEM30, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                gem_controller.Get_Gem(10);// The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
            }
            
            else if (String.Equals(args.purchasedProduct.definition.id, GEM70, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                gem_controller.Get_Gem(11);
            }
            
            
            else if (String.Equals(args.purchasedProduct.definition.id, GEM300, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                gem_controller.Get_Gem(12);
            }
            
            else if (String.Equals(args.purchasedProduct.definition.id, GEM700, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                gem_controller.Get_Gem(13);
            }
            
            else if (String.Equals(args.purchasedProduct.definition.id, GEM1500, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                gem_controller.Get_Gem(14);
            }
            
            else if (String.Equals(args.purchasedProduct.definition.id, GEM3400, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                gem_controller.Get_Gem(15);
            }
            
            else if (String.Equals(args.purchasedProduct.definition.id, Starter, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                gem_controller.Get_Gem(101);
                _panelControl.Determine_Avatar_On();
            }
            
            else if ((String.Equals(args.purchasedProduct.definition.id, Astronaut, StringComparison.Ordinal)) 
                     ||(String.Equals(args.purchasedProduct.definition.id, astro_sale, StringComparison.Ordinal)))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                _avatarPackage.Get_Package(3);
                _panelControl.Determine_Avatar_On();
                if(String.Equals(args.purchasedProduct.definition.id, astro_sale, StringComparison.Ordinal))
                    _panelControl.Set_SuperSale();
            }
            
            else if ((String.Equals(args.purchasedProduct.definition.id, Bear, StringComparison.Ordinal)) 
                     || (String.Equals(args.purchasedProduct.definition.id, bear_sale, StringComparison.Ordinal)))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                _avatarPackage.Get_Package(4);
                _panelControl.Determine_Avatar_On();
                if(String.Equals(args.purchasedProduct.definition.id, bear_sale, StringComparison.Ordinal))
                    _panelControl.Set_SuperSale();
            }
            
            else if ((String.Equals(args.purchasedProduct.definition.id, Party, StringComparison.Ordinal))
                     ||(String.Equals(args.purchasedProduct.definition.id, party_sale, StringComparison.Ordinal)))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                _avatarPackage.Get_Package(2);
                _panelControl.Determine_Avatar_On();
                if(String.Equals(args.purchasedProduct.definition.id, party_sale, StringComparison.Ordinal))
                    _panelControl.Set_SuperSale();
            }
            
            else if (String.Equals(args.purchasedProduct.definition.id, Noads, StringComparison.Ordinal))
            {
                Noads_instance.Set_Is_Noads();
                _panelControl.Purchase_Noads();
                _panelControl.Determine_Avatar_On();
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
            }
            
            else if ((String.Equals(args.purchasedProduct.definition.id, Science, StringComparison.Ordinal))
                     ||(String.Equals(args.purchasedProduct.definition.id, science_sale, StringComparison.Ordinal)))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                _avatarPackage.Get_Package(5);
                _panelControl.Determine_Avatar_On();
                if(String.Equals(args.purchasedProduct.definition.id, science_sale, StringComparison.Ordinal))
                    _panelControl.Set_SuperSale();
            }

            else 
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }
    }
}