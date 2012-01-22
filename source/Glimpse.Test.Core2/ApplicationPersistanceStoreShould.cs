﻿using System;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core2.Framework;
using Glimpse.Test.Core2.Tester;
using Xunit;

namespace Glimpse.Test.Core2
{
    public class ApplicationPersistanceStoreShould:IDisposable
    {
        private ApplicationPersistanceStoreTester tester;
        public ApplicationPersistanceStoreTester Store
        {
            get { return tester ?? (tester = ApplicationPersistanceStoreTester.Create()); }
            set { tester = value; }
        }

        public void Dispose()
        {
            Store = null;
        }

        [Fact]
        public void Construct()
        {
            Assert.NotNull(Store);
        }

        [Fact]
        public void Persist()
        {
            Assert.Equal(0, Store.GlimpseRequests.Count());

            var pluginData = new Dictionary<string, string>();

            Store.Save(new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0));

            Assert.Equal(1, Store.GlimpseRequests.Count());
        }

        [Fact]
        public void GetGlimpseMetadataById()
        {
            var requestId = Guid.NewGuid();
            var metadata = new GlimpseMetadata(requestId, Store.RequestMetadataMock.Object, new Dictionary<string, string>(), 0);

            Store.Save(metadata);

            var result = Store.GetByRequestId(requestId);

            Assert.Equal(metadata, result);
        }

        [Fact]
        public void GetGlimpseMetadataByIdWithMisMatch()
        {
            var requestId = Guid.Parse("00000000-0000-0000-0000-000000000000");

            Store.Save(new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, new Dictionary<string, string>(), 0));

            Assert.Null(Store.GetByRequestId(requestId));
        }

        [Fact]
        public void GetGlimpseMetadataByParentRequestId()
        {
            var pluginData = new Dictionary<string, string>();

            Store.Save(new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0));

            Assert.Equal(1, Store.GetByRequestParentId(Guid.Parse("936DA01F-9ABD-4d9d-80C7-02AF85C822A8")).Count());
        }

        [Fact]
        public void GetGlimpseMetadataByParentRequestIdWithMisMatch()
        {
            var pluginData = new Dictionary<string, string>();

            Store.Save(new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0));

            Assert.Equal(0,Store.GetByRequestParentId(Guid.Parse("00000000-0000-0000-0000-000000000000")).Count());
        }

        [Fact]
        public void GetTopWithFewerThanRequested()
        {
            var pluginData = new Dictionary<string, string>();


            var metadata1 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);
            var metadata2 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);
            var metadata3 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);
            var metadata4 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);
            var metadata5 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);

            Store.Save(metadata1);
            Store.Save(metadata2);
            Store.Save(metadata3);
            Store.Save(metadata4);
            Store.Save(metadata5);

            var result = Store.GetTop(10);
            Assert.Equal(5, result.Count());
            Assert.Equal(metadata1, result[0]);
            Assert.Equal(metadata2, result[1]);
            Assert.Equal(metadata3, result[2]);
            Assert.Equal(metadata4, result[3]);
            Assert.Equal(metadata5, result[4]);
        }

        [Fact]
        public void GetTopWithMoreThanRequested()
        {
            var pluginData = new Dictionary<string, string>();


            var metadata1 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);
            var metadata2 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);
            var metadata3 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);
            var metadata4 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);
            var metadata5 = new GlimpseMetadata(Guid.NewGuid(), Store.RequestMetadataMock.Object, pluginData, 0);

            Store.Save(metadata1);
            Store.Save(metadata2);
            Store.Save(metadata3);
            Store.Save(metadata4);
            Store.Save(metadata5);

            var result = Store.GetTop(3);
            Assert.Equal(3, result.Count());
            Assert.Equal(metadata1, result[0]);
            Assert.Equal(metadata2, result[1]);
            Assert.Equal(metadata3, result[2]);
        }

        [Fact]
        public void GetTopWithNoResults()
        {
            var result = Store.GetTop(3);
            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void ThrowWithEmptyTabKey()
        {
            Assert.Throws<ArgumentNullException>(()=>Store.GetByRequestIdAndTabKey(Guid.NewGuid(), string.Empty));
        }

        [Fact]
        public void GetOnePluginsData()
        {

            var key = "theKey";
            var value = "theValue";
            var pluginData = new Dictionary<string, string>
                                 {
                                     {key,value}
                                 };


            var id = Guid.NewGuid();
            var metadata = new GlimpseMetadata(id, Store.RequestMetadataMock.Object, pluginData, 0);

            Store.Save(metadata);

            var result = Store.GetByRequestIdAndTabKey(id, key);
            Assert.Equal(value, result);
        }

        [Fact]
        public void GetOnePluginsDataReturnsNullWithMisMatchKey()
        {

            var key = "theKey";
            var value = "theValue";
            var pluginData = new Dictionary<string, string>
                                 {
                                     {key,value}
                                 };


            var id = Guid.NewGuid();
            var metadata = new GlimpseMetadata(id, Store.RequestMetadataMock.Object, pluginData, 0);

            Store.Save(metadata);

            var result = Store.GetByRequestIdAndTabKey(id, "wrongKey");
            Assert.Null(result);
        }

        [Fact]
        public void GetOnePluginsDataReturnsNullWithMisMatchId()
        {

            var key = "theKey";
            var value = "theValue";
            var pluginData = new Dictionary<string, string>
                                 {
                                     {key,value}
                                 };


            var id = Guid.NewGuid();
            var metadata = new GlimpseMetadata(id, Store.RequestMetadataMock.Object, pluginData, 0);

            Store.Save(metadata);

            var result = Store.GetByRequestIdAndTabKey(Guid.Parse("00000000-0000-0000-0000-000000000000"), key);
            Assert.Null(result);
        }
    }
}