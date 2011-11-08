using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Rackspace.CloudFiles.Domain;
using Rackspace.CloudFiles.Domain.Request;

namespace Rackspace.CloudFiles
{
    public interface IConnection
    {
        AccountInformation GetAccountInformation();
        string GetAccountInformationJson();
        Boolean HasCDN();
        XmlDocument GetAccountInformationXml();
        void CreateContainer(string containerName);
        void CreateContainer(string containerName, Dictionary<string, string> metadata);
        void DeleteContainer(string continerName);
        void DeleteContainer(string continerName, bool emptyContainerBeforeDelete);
        void PurgePublicContainer(string continerName, string[] emailAddresses);
        void PurgePublicContainer(string continerName);
        List<string> GetContainers();
        List<string> GetContainers(Dictionary<GetListParameters, string> parameters);
        List<string> GetContainerItemList(string containerName);
        List<string> GetContainerItemList(string containerName, bool includeFolders);
        List<string> GetContainerItemList(string containerName, Dictionary<GetListParameters, string> parameters);
        List<string> GetContainerItemList(string containerName, Dictionary<GetListParameters, string> parameters, bool includeFolders);
        Container GetContainerInformation(string containerName);
        string GetContainerInformationJson(string containerName);
        XmlDocument GetContainerInformationXml(string containerName);
        void PutStorageItemAsync(string containerName, Stream storageStream, string remoteStorageItemName);
        void PutStorageItemAsync(string containerName, string localStorageItemName);
        void GetStorageItemAsync(string containerName, string storageItemName, string localItemName);
        void PutStorageItem(string containerName, string localFilePath, Dictionary<string, string> metadata);
        void PutStorageItem(string containerName, string localFilePath);
        void PutStorageItem(string containerName, string localFilePath, string remoteStorageItemName);
        void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName);
        void PutStorageItem(string containerName, Stream storageStream, string remoteStorageItemName, Dictionary<string, string> metadata);
        void DeleteStorageItem(string containerName, string storageItemname);
        void PurgePublicStorageItem(string containerName, string storageItemname);
        StorageItem GetStorageItem(string containerName, string storageItemName);
        void GetStorageItem(string containerName, string storageItemName, string localFileName);
        StorageItem GetStorageItem(string containerName, string storageItemName, Dictionary<RequestHeaderFields, string> requestHeaderFields);
        void GetStorageItem(string containerName, string storageItemName, string localFileName, Dictionary<RequestHeaderFields, string> requestHeaderFields);
        StorageItemInformation GetStorageItemInformation(string containerName, string storageItemName);
        void SetStorageItemMetaInformation(string containerName, string storageItemName, Dictionary<string, string> metadata);
        List<string> GetPublicContainers();
        Uri MarkContainerAsPublic(string containerName);
        Uri MarkContainerAsPublic(string containerName, int timeToLiveInSeconds);
        void MarkContainerAsPrivate(string containerName);
       
        Container GetPublicContainerInformation(string containerName);
        void MakePath(string containerName, string path);
        IAccount Account { get; }

        /// <summary>
        /// The storage url used to interact with cloud files
        /// </summary>
        string StorageUrl { get;  }

        /// <summary>
        /// the session based token used to ensure the user was authenticated
        /// </summary>
        string AuthToken { get;  }


        void SetDetailsOnPublicContainer(string publiccontainer, bool loggingenabled, int ttl);
        XmlDocument GetPublicAccountInformationXML();
        string GetPublicAccountInformationJSON();
    }
}